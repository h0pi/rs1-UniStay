using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;



[ApiController]
[Route("api/account")]
public class AccountSecuritySendEmailTokenEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecuritySendEmailTokenEndpoint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // 5) send email token (classic)
    [HttpPost("password/send-email-token")]
    public async Task<IActionResult> SendEmailToken([FromQuery] string email)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return NotFound();

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

        await _emailService.SendPasswordResetTokensAsync(user.Email, token);
        return Ok();
    }

}