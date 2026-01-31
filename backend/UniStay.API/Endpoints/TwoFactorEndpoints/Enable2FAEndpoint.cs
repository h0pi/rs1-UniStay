using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/account/2fa")]
public class Enable2FAEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITwoFactorService _twoFactor;
    public Enable2FAEndpoint(ApplicationDbContext db, ITwoFactorService twoFactor)
    {
        _db = db;
        _twoFactor = twoFactor;
    }

    [HttpPost("enable")]
    public async Task<IActionResult> Enable([FromBody] Enable2FaDto dto)
    {
        var user = await _db.User.FindAsync(dto.UserId);
        if (user == null) return NotFound();

        var s = await _db.TwoFactorSettings.FindAsync(dto.UserId);
        if (s == null)
        {
            s = new TwoFactorSettings { UserID = dto.UserId, IsEnabled = true,RequiresTwoFactor=true, EnabledAt = DateTime.UtcNow, Method = "email" };
            _db.TwoFactorSettings.Add(s);
        }
        else
        {
            s.IsEnabled = true;
            s.RequiresTwoFactor = true;
            s.EnabledAt = DateTime.UtcNow;
            s.Method = "email";
        }
        await _db.SaveChangesAsync();

        // generate backup codes and return plain list to user
        var backup = await _twoFactor.GenerateBackupCodesAsync(dto.UserId);
        return Ok(new { backupCodes = backup });
    }

    // DTOs:
    public class Enable2FaDto { public int UserId { get; set; } }

}