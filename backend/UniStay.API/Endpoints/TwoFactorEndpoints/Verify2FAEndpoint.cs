//using Microsoft.AspNetCore.Mvc;
//using UniStay.API.Data;
//using UniStay.API.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using UniStay.API.Services;
//using MyAuthInfo = UniStay.API.Data.Models.MyAuthInfo;   // ⭐ DODANO

//[ApiController]
//[Route("api/account/2fa")]
//public class Verify2FAEndpoint : ControllerBase
//{
//    private readonly ApplicationDbContext _db;
//    private readonly ITwoFactorService _twoFactor;
//    private readonly IMyAuthService _auth;   // ⭐ DODANO

//    public Verify2FAEndpoint(ApplicationDbContext db, ITwoFactorService twoFactor, IMyAuthService auth) // ⭐ DODANO auth
//    {
//        _db = db;
//        _twoFactor = twoFactor;
//        _auth = auth;  // ⭐
//    }

//    [HttpPost("verify")]
//    public async Task<IActionResult> Verify([FromBody] Verify2FaDto dto)
//    {
//        // 1) Verify code
//        var ok = await _twoFactor.VerifyTwoFactorCodeAsync(dto.UserId, dto.Code);
//        if (!ok)
//        {
//            var okBackup = await _twoFactor.VerifyBackupCodeAsync(dto.UserId, dto.Code);
//            if (!okBackup)
//                return BadRequest("Invalid or expired code.");
//        }

//        // 🔴 OVDJE DODATI — NAKON što je 2FA kod VALIDAN

//        var authRow = await _db.MyAuthInfo
//            .FirstOrDefaultAsync(x => x.UserId == dto.UserId);

//        if (authRow == null)
//        {
//            // ako red ne postoji → kreiraj ga
//            authRow = new MyAuthInfo
//            {
//                UserId = dto.UserId,
//                IsLoggedIn = true
//            };
//            _db.MyAuthInfo.Add(authRow);
//        }
//        else
//        {
//            // ako postoji → samo postavi na 1
//            authRow.IsLoggedIn = true;
//        }

//        await _db.SaveChangesAsync();

//        // 2) Get User
//        var user = await _db.User.FirstOrDefaultAsync(x => x.UserID == dto.UserId);
//        if (user == null)
//            return Unauthorized("User not found.");

//        // 3) Generate final auth token (REAL LOGIN TOKEN)
//        //var tokenObj = await _auth.GenerateAndSaveAuthToken(user);
//        //var authInfo = _auth.GetAuthInfoFromTokenString(tokenObj.Value);

//        var authInfo = await _db.MyAuthInfo
//            .FirstOrDefaultAsync(x => x.UserId == user.UserID);

//        if (authInfo == null)
//        {
//            authInfo = new MyAuthInfo
//            {
//                UserId = user.UserID,
//                Email = user.Email,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                RoleName = user.Role?.RoleName,
//                RoleId = user.RoleID,
//                IsLoggedIn = true
//            };

//            _db.MyAuthInfo.Add(authInfo);
//        }
//        else
//        {
//            authInfo.IsLoggedIn = true;
//        }

//        await _db.SaveChangesAsync();

//        // 4️⃣ Generiši FINALNI TOKEN
//        var tokenObj = await _auth.GenerateAndSaveAuthToken(user);
//        var authInfoFromToken = _auth.GetAuthInfoFromTokenString(tokenObj.Value);

//        // 4) Send token + role back to frontend
//        return Ok(new
//        {
//            token = tokenObj.Value,
//            //myAuthInfo = authInfo
//            myAuth=authInfoFromToken
//        });
//    }

//    // DTOs:
//    public class Verify2FaDto
//    {
//        public int UserId { get; set; }
//        public string Code { get; set; }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Services;

[ApiController]
[Route("api/account/2fa")]
public class Verify2FAEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITwoFactorService _twoFactor;
    private readonly IMyAuthService _auth;

    public Verify2FAEndpoint(
        ApplicationDbContext db,
        ITwoFactorService twoFactor,
        IMyAuthService auth)
    {
        _db = db;
        _twoFactor = twoFactor;
        _auth = auth;
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] Verify2FaDto dto)
    {
        // 1️⃣ VERIFY 2FA (NE DIRATI)
        var ok = await _twoFactor.VerifyTwoFactorCodeAsync(dto.UserId, dto.Code);
        if (!ok)
        {
            var okBackup = await _twoFactor.VerifyBackupCodeAsync(dto.UserId, dto.Code);
            if (!okBackup)
                return BadRequest("Invalid or expired code.");
        }

        // 2️⃣ GET USER (NE DIRATI)
        var user = await _db.User.FirstOrDefaultAsync(x => x.UserID == dto.UserId);
        if (user == null)
            return Unauthorized("User not found.");

        // 3️⃣ GENERATE TOKEN (NE DIRATI)
        var tokenObj = await _auth.GenerateAndSaveAuthToken(user);
        var authInfo = _auth.GetAuthInfoFromTokenString(tokenObj.Value);

        // 🔥🔥🔥 JEDINA STVAR KOJU DODAJEMO 🔥🔥🔥
        // OZNAČI USERA KAO LOGOVANOG U BAZI
        await _db.Database.ExecuteSqlRawAsync(
            "UPDATE MyAuthInfo SET IsLoggedIn = 1 WHERE UserId = {0}",
            dto.UserId
        );

        // 4️⃣ RESPONSE (NE DIRATI)
        return Ok(new
        {
            token = tokenObj.Value,
            myAuthInfo = authInfo
        });
    }

    public class Verify2FaDto
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }
}