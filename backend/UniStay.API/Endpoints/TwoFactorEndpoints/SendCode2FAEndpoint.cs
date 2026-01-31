using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/account/2fa")]
public class SendCode2FAEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITwoFactorService _twoFactor;
    public SendCode2FAEndpoint(ApplicationDbContext db, ITwoFactorService twoFactor)
    {
        _db = db;
        _twoFactor = twoFactor;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendCode([FromBody] Send2FaDto dto)
    {
        // dto contains userId or email
        var user = await _db.User.FindAsync(dto.UserId);
        if (user == null) return NotFound();
        await _twoFactor.SendTwoFactorCodeAsync(user.UserID, user.Email);
        return Ok();
    }

    // DTOs:
    public class Send2FaDto { public int UserId { get; set; } }
}