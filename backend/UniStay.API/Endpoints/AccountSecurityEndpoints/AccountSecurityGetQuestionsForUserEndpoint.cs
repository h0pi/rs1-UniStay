using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;

[ApiController]
[Route("api/account")]
public class AccountSecurityGetQuestionsForUserEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecurityGetQuestionsForUserEndpoint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // 3) get questions for given user (only question text - no answers)
    [HttpGet("security/questions-for-user")]
    public async Task<IActionResult> GetQuestionsForUser([FromQuery] string email)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return NotFound();

        var list = await _db.UserSecurityAnswers
            .Where(a => a.UserID == user.UserID)
            .Include(a => a.SecurityQuestion)
            .Select(a => new SecurityQuestionsResponseDto
            {
                QuestionId = a.SecurityQuestionID,
                Question = a.SecurityQuestion.Text
            })
            .ToListAsync();

        // if no questions set => inform client
        if (!list.Any()) return BadRequest("No security questions set for this account");

        return Ok(list);
    }


}