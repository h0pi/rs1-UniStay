using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Account;

[ApiController]
[Route("api/account")]
public class AccountSecurityResetPasswordEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService; // optional, implement in your project
    public AccountSecurityResetPasswordEndpoint(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }


    // 6) reset password with token
    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
    {
        var prt = await _db.PasswordResetTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Token == dto.Token && !t.Used);
        if (prt == null || prt.ExpiresAt < DateTime.UtcNow) return BadRequest("Invalid or expired token");

        // hash new password and save to user
        var user = prt.User;
        // assume you have Password hashing flow - example using PasswordHasher<Users>
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Users>();
        user.PasswordHash = hasher.HashPassword(user, dto.NewPassword); // adjust: your Users model might have PasswordHash or Password field

        prt.Used = true;
        await _db.SaveChangesAsync();
        return Ok();
    }
}