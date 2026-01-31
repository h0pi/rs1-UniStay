using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using DataToken = UniStay.API.Data.Models.MyAuthenticationToken;

namespace UniStay.API.Services
{
    public class MyAuthService : IMyAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyAuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DataToken> GenerateAndSaveAuthToken(Users user, CancellationToken cancellationToken = default)
        {
            string tokenValue = Guid.NewGuid().ToString("N"); // jednostavan random token

            var token = new DataToken
            {
                Value = tokenValue,
                IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "",
                User = user,
                RecordedAt = DateTime.Now
            };

            _context.Add(token);
            await _context.SaveChangesAsync(cancellationToken);

            return token;
        }

        public async Task<bool> RevokeAuthToken(string tokenValue, CancellationToken cancellationToken = default)
        {
            var token = await _context.Set<DataToken>().FirstOrDefaultAsync(t => t.Value == tokenValue, cancellationToken);
            if (token == null)
                return false;

            _context.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Data.Models.MyAuthInfo GetAuthInfoFromTokenString(string? tokenValue)
        {
            if (string.IsNullOrEmpty(tokenValue))
                return new Data.Models.MyAuthInfo();

            var token = _context.Set<DataToken>()
                .Include(t => t.User)
                .ThenInclude(u => u.Role) 
                .FirstOrDefault(t => t.Value == tokenValue);

            if (token == null)
                return new Data.Models.MyAuthInfo();

            return new Data.Models.MyAuthInfo
            {
                UserId = token.UserID,
                Email = token.User.Email,
                FirstName = token.User.FirstName,
                LastName = token.User.LastName,
                RoleName = token.User.Role?.RoleName,
                IsLoggedIn = true
            };
        }

        public Data.Models.MyAuthInfo GetAuthInfoFromRequest()
        {
            //var token = _httpContextAccessor.HttpContext?.Request.Headers["auth-token"].ToString();
            //return GetAuthInfoFromTokenString(token);
            var header = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var token = header?.Replace("Bearer ", "");
            return GetAuthInfoFromTokenString(token);
        }
    }
}
