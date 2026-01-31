using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/account/2fa")]
public class Disable2FAEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITwoFactorService _twoFactor;
    public Disable2FAEndpoint(ApplicationDbContext db, ITwoFactorService twoFactor)
    {
        _db = db;
        _twoFactor = twoFactor;
    }

    [HttpPost("disable")]
    public async Task<IActionResult> Disable([FromBody] Disable2FaDto dto)
    {
        await _twoFactor.DisableTwoFactorAsync(dto.UserId);
        return Ok();
    }

    // DTOs:

    public class Disable2FaDto { public int UserId { get; set; } }

}