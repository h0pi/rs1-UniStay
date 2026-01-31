using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;

[ApiController]
[Route("api/account")]
public class AccountSecuritySetUserAnswersEndpint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecuritySetUserAnswersEndpint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // 2) set user's answers (profile flow)
    [HttpPost("security/set")]
    public async Task<IActionResult> SetUserAnswers([FromBody] SetSecurityAnswersDto dto)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return NotFound("User not found");

        // remove existing
        var existing = _db.UserSecurityAnswers.Where(a => a.UserID == user.UserID);
        _db.UserSecurityAnswers.RemoveRange(existing);

        foreach (var a in dto.Answers)
        {
            var hashed = SecurityHelpers.HashAnswer(user, a.Answer);
            _db.UserSecurityAnswers.Add(new UserSecurityAnswer
            {
                UserID = user.UserID,
                SecurityQuestionID = a.QuestionId,
                AnswerHash = hashed
            });
        }

        await _db.SaveChangesAsync();
        return Ok();
    }
}