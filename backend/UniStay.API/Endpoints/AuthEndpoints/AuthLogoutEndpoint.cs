using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using UniStay.API.Services;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static UniStay.API.Endpoints.AuthEndpoints.AuthLogoutEndpoint;

namespace UniStay.API.Endpoints.AuthEndpoints;

[Route("api/auth")]
public class AuthLogoutEndpoint(ApplicationDbContext db, IMyAuthService authService)
    : MyEndpointBaseAsync
        .WithoutRequest
        .WithActionResult<LogoutResponse>
{
    [HttpPost("logout")]
    public override async Task<ActionResult<LogoutResponse>> HandleAsync(
    CancellationToken cancellationToken = default)
    {
        // 1️⃣ Uzmi token kao string (isto kao login koristi)
        var header = Request.Headers["Authorization"].ToString();
        var authToken = header.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(authToken))
        {
            return BadRequest(new LogoutResponse
            {
                IsSuccess = false,
                Message = "Missing token."
            });
        }

        // 2️⃣ Izvuci UserId IZ TOKEN TABELE (NE iz JWT claimova)
        var tokenEntity = await db.MyAuthenticationTokens
            .FirstOrDefaultAsync(x => x.Value == authToken, cancellationToken);

        if (tokenEntity != null)
        {
            // 3️⃣ IDENTIČNO KAO LOGIN → ali na 0
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE MyAuthInfo SET IsLoggedIn = 0 WHERE UserId = {0}",
                tokenEntity.UserID
            );
        }

        // 4️⃣ Revoke token (ostaje kako već imaš)
        bool isRevoked = await authService.RevokeAuthToken(authToken, cancellationToken);

        return Ok(new LogoutResponse
        {
            IsSuccess = isRevoked,
            Message = "Logout successful."
        });
    }
}

    public class LogoutResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }

