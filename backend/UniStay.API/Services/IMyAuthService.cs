using Microsoft.AspNetCore.Authentication;
using UniStay.API.Data.Models;
using MyAuthenticationTokenModel = UniStay.API.Data.Models.MyAuthenticationToken;

namespace UniStay.API.Services
{
    public interface IMyAuthService
    {
        Task<MyAuthenticationTokenModel> GenerateAndSaveAuthToken(Users user, CancellationToken cancellationToken = default);
        Task<bool> RevokeAuthToken(string tokenValue, CancellationToken cancellationToken = default);
        Data.Models.MyAuthInfo GetAuthInfoFromTokenString(string? tokenValue);
        Data.Models.MyAuthInfo GetAuthInfoFromRequest();
    }
}
