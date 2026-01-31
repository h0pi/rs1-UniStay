using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Helper.Api;
using UniStay.API.Services;
using System.Threading;
using System.Threading.Tasks;
using static UniStay.API.Endpoints.AuthEndpoints.AuthLoginEndpoint;
using AuthInfo = UniStay.API.Data.Models.MyAuthInfo;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyAuthInfo = UniStay.API.Data.Models.MyAuthInfo;
using Microsoft.Data.SqlClient;
using UniStay.API.Migrations;

namespace UniStay.API.Endpoints.AuthEndpoints;

[Route("api/auth")]
public class AuthLoginEndpoint(ApplicationDbContext db, IMyAuthService authService, ITwoFactorService twoFactorService, IConfiguration config)
    : MyEndpointBaseAsync
        .WithRequest<LoginRequest>
        .WithActionResult<LoginResponse>
{
    [HttpPost("login")]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await db.User.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);


        Console.WriteLine($"[LOGIN] Pronadjen korisnik: {user?.Email}, ID: {user?.UserID}");

        using var sha = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(request.Password);
        string passwordHash = Convert.ToBase64String(sha.ComputeHash(passwordBytes));

        if (user == null || user.PasswordHash != passwordHash)
            return Unauthorized(new { Message = "Invalid email or password." });



        Console.WriteLine($"[DEBUG] Connection string: {db.Database.GetConnectionString()}");

        var requires2FA = await db.User.AnyAsync(s => s.UserID == user.UserID, cancellationToken);

        var settings = await db.TwoFactorSettings.FirstOrDefaultAsync(x => x.UserID == user.UserID);

        // ⭐ FIXED – 2FA RESPONSE KORISTI LoginResponse
        if (requires2FA)
        {
           // if (settings != null && settings.RequiresTwoFactor)
          //  {
                await twoFactorService.GenerateAndSendCode(user.UserID);

                return Ok(new LoginResponse
                {
                    RequiresTwoFactor = requires2FA,   // ⭐ FIXED → camelCase for Angular
                    UserId = user.UserID,
                    Email = user.Email,         // ⭐ FIXED – dodano za frontend
                    Token = "",                 // ⭐ MUST EXIST (LoginResponse expects it)
                    MyAuthInfo = null,          // ⭐ NOW allowed because nullable
                    TwoFactorEnabled = requires2FA
                });
          //  }
        }

        // Normal login:
        //var newAuthToken = await authService.GenerateAndSaveAuthToken(user, cancellationToken);
        //var authInfo = authService.GetAuthInfoFromTokenString(newAuthToken.Value);
        //var newAuthToken = await authService.GenerateAndSaveAuthToken(user, cancellationToken);
        //var authInfo = authService.GetAuthInfoFromTokenString(newAuthToken.Value);

        //return Ok(new LoginResponse
        //{
        //    Token = newAuthToken.Value,
        //    MyAuthInfo = authInfo,
        //    UserId = user.UserID,
        //    TwoFactorEnabled = settings?.RequiresTwoFactor ?? false,
        //    RequiresTwoFactor = false,
        //    Email = user.Email       // ⭐ FIXED – added for compatibility
        //});





        var claims = new[]
        {
    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // ⭐ KLJUČNO
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role.RoleName)
};

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

        var authInfo = await db.MyAuthInfo.FirstOrDefaultAsync(a => a.UserId == user.UserID, cancellationToken); if (authInfo == null)
        { authInfo = new MyAuthInfo { UserId = user.UserID, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, RoleName = user.Role.RoleName, IsLoggedIn = true }; 
            db.MyAuthInfo.Add(authInfo); } else {  authInfo.IsLoggedIn = true; } await db.SaveChangesAsync(cancellationToken);

            return Ok(new LoginResponse
        {
            Token = jwtString,
            MyAuthInfo = null, // ili ostavi ako ti treba
            UserId = user.UserID,
            TwoFactorEnabled = settings?.RequiresTwoFactor ?? false,
            RequiresTwoFactor = false,
            Email = user.Email
        });

    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public AuthInfo? MyAuthInfo { get; set; }   // ⭐ FIXED nullable
        public string? Token { get; set; }          // ⭐ FIXED nullable
        public bool TwoFactorEnabled { get; set; }
        public bool RequiresTwoFactor { get; set; } // ⭐ FIXED name (camelCase)
        public string? Email { get; set; }          // ⭐ Added
    }
}


//    var authInfo = await db.MyAuthInfo.FirstOrDefaultAsync(x => x.UserId == user.UserID);

//    if (authInfo != null)
//    {
//        authInfo.IsLoggedIn = true;

//    }
//    await db.SaveChangesAsync();
//authInfo = new MyAuthInfo
//{
//    UserId = user.UserID,
//    IsLoggedIn = true
//};
//    db.MyAuthInfo.Add(authInfo);
//}
//else
//{
//    authInfo.IsLoggedIn = true;
//}

//await db.SaveChangesAsync();


//// ✅ OZNAČI USERA KAO LOGOVANOG

//// ✅ OZNAČI USERA KAO LOGOVANOG
////var authInfo = await db.MyAuthInfo
////    .FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

////if (authInfo == null)
////{
////    // Ako ne postoji zapis za tog korisnika → kreiraj novi
////    authInfo = new MyAuthInfo
////    {
////        UserId = user.UserID,
////        IsLoggedIn = true   // 1 = prijavljen
////    };
////    db.MyAuthInfo.Add(authInfo);
////}
////else
////{
////    // Ako postoji → samo ažuriraj status
////    authInfo.IsLoggedIn = true;
////}

////await db.SaveChangesAsync(cancellationToken);



////    var authInfo = await db.MyAuthInfo
////.FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

////    if (authInfo == null)
////    {
////        authInfo = new MyAuthInfo
////        {
////            UserId = user.UserID,
////            FirstName = user.FirstName,
////            LastName = user.LastName,
////            Email = user.Email,
////            RoleName = user.Role.RoleName,
////            RoleId = user.Role.RoleID,
////            IsLoggedIn = true   // ✅ bool → u bazi će biti 1
////        };
////        db.MyAuthInfo.Add(authInfo);
////    }
////    else
////    {
////        authInfo.IsLoggedIn = true;
////    }

////    await db.SaveChangesAsync(cancellationToken);

////       var authInfo = await db.MyAuthInfo
////.FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

////       if (authInfo == null)
////       {
////           authInfo = new MyAuthInfo
////           {
////               UserId = user.UserID,
////               IsLoggedIn = true
////           };
////           db.MyAuthInfo.Add(authInfo);
////       }
////       else
////       {
////           authInfo.IsLoggedIn = true;
////       }

////       await db.SaveChangesAsync(cancellationToken);

////        await db.Database.ExecuteSqlRawAsync(@"
////MERGE MyAuthInfo AS target
////USING (SELECT {0} AS UserId) AS source
////ON target.UserId = source.UserId
////WHEN MATCHED THEN
////    UPDATE SET IsLoggedIn = 1
////WHEN NOT MATCHED THEN
////    INSERT (UserId, IsLoggedIn)
////    VALUES (source.UserId, 1);
////", user.UserID);


////    var authInfo = await db.MyAuthInfo
////.FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

////    if (authInfo != null)
////    {
////        authInfo.IsLoggedIn = true; // prijava → 1
////        db.Entry(authInfo).Property(x => x.IsLoggedIn).IsModified = true;
////        await db.SaveChangesAsync(cancellationToken);
////        await db.Database.ExecuteSqlRawAsync(
////"UPDATE MyAuthInfo SET IsLoggedIn = 1 WHERE UserId = {0}", user.UserID);

////    }

////var authInfo = await db.MyAuthInfo
////    .FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

////if (authInfo != null)
////{
////    // Forsiraj attach i update
////    db.Attach(authInfo);
////    authInfo.IsLoggedIn = true;
////    db.Entry(authInfo).Property(x => x.IsLoggedIn).IsModified = true;

////    try
////    {
////        await db.SaveChangesAsync(cancellationToken);
////        Console.WriteLine("[DEBUG] IsLoggedIn update poslan u bazu.");
////    }
////    catch (Exception ex)
////    {
////        Console.WriteLine($"[ERROR] SaveChanges nije uspio: {ex.Message}");
////    }
////}

//// direktno SQL update, bez EF trackinga

//var authInfo = await db.MyAuthInfo
//  .FirstOrDefaultAsync(x => x.UserId == user.UserID, cancellationToken);

//if (authInfo != null)
//{
//    // Forsiraj update
//    authInfo.IsLoggedIn = true;
//    db.Entry(authInfo).Property(x => x.IsLoggedIn).IsModified = true;

//    var affected = await db.SaveChangesAsync(cancellationToken);
//    Console.WriteLine($"[DEBUG] Rows affected: {affected}");
//}




/////////////////////////////////////////////////////////////////////////