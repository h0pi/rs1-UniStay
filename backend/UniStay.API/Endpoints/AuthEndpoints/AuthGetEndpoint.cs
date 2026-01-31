using Microsoft.AspNetCore.Mvc;
using UniStay.API.Helper.Api;
using UniStay.API.Services;
using System.Threading;
using System.Threading.Tasks;
using static UniStay.API.Endpoints.AuthEndpoints.AuthGetEndpoint;
using UniStay.API.Data.Models;
using AuthInfo = UniStay.API.Data.Models.MyAuthInfo;


namespace UniStay.API.Endpoints.AuthEndpoints;

[Route("api/auth")]
public class AuthGetEndpoint(IMyAuthService authService) : MyEndpointBaseAsync
    .WithoutRequest
    .WithActionResult<AuthGetResponse>
{
    [HttpGet("me")]
    public override async Task<ActionResult<AuthGetResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var authInfo = authService.GetAuthInfoFromRequest();

        if (!authInfo.IsLoggedIn)
            return Unauthorized("Invalid or expired token");

        return Ok(new AuthGetResponse { MyAuthInfo = authInfo });
    }

    public class AuthGetResponse
    {
        public required AuthInfo MyAuthInfo { get; set; }
    }
}
