using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;

[ApiController]
[Route("api/account")]
public class AccountSecurityGetAllQuestionsEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecurityGetAllQuestionsEndpoint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // 1) get available questions (admin/seed these once)
    [HttpGet("security/questions")]
    public async Task<IActionResult> GetAllQuestions()
    {
        var qs = await _db.SecurityQuestions
            .Select(q => new { q.SecurityQuestionID, q.Text })
            .ToListAsync();
        return Ok(qs);
    }

}