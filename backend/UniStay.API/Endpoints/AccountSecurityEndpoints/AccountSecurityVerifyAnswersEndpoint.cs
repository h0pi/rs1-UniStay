using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;

[ApiController]
[Route("api/account")]
public class AccountSecurityVerifyAnswersEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecurityVerifyAnswersEndpoint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // 4) verify answers => create password reset token (returned or emailed)
    [HttpPost("security/verify")]
    public async Task<IActionResult> VerifyAnswers([FromBody] VerifyAnswersDto dto)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return NotFound();

        var stored = await _db.UserSecurityAnswers.Where(a => a.UserID == user.UserID).ToListAsync();
        if (!stored.Any()) return BadRequest("No security questions set");

        // check counts
        if (dto.Answers.Count != stored.Count)
            return BadRequest("Answers count mismatch");

        // verify all: for each provided answer find matching question and verify
        foreach (var provided in dto.Answers)
        {
            var s = stored.FirstOrDefault(x => x.SecurityQuestionID == provided.QuestionId);
            if (s == null) return BadRequest("Invalid question id");
            var res = SecurityHelpers.VerifyAnswer(user, s.AnswerHash, provided.Answer);
            if (res == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return BadRequest("Answers do not match");
        }

        // generate token
        var token = SecurityHelpers.GenerateSecureToken(32);
        var prt = new PasswordResetToken
        {
            UserID = user.UserID,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            Used = false,
            CreatedAt = DateTime.UtcNow
        };
        _db.PasswordResetTokens.Add(prt);
        await _db.SaveChangesAsync();

        // Optionally: email token to user
        await _emailService.SendPasswordResetTokensAsync(user.Email, token);

        return Ok(new VerifyResultDto { Success = true, ResetToken = token }); // in prod you may omit ResetToken to force email-only
    }

 
}