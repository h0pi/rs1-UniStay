using Microsoft.AspNetCore.Mvc;
using static UniStay.API.Endpoints.UserEndpoints.UserGetAllEndpoint;
using UniStay.API.Data.Models.Dto.User;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using UniStay.API.Helper;
using UniStay.API.Services;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.UserEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    //[MyAuthorization("Admin")]
    public class UserGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<UserGetAllRequest>
        .WithResult<MyPagedList<UserDTO>>
    {
        [HttpGet("filter")]
        public override async Task<MyPagedList<UserDTO>> HandleAsync(
            [FromQuery] UserGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = db.User
                .Include(u => u.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(request.Q) ||
                    u.LastName.Contains(request.Q) ||
                    u.Username.Contains(request.Q) ||
                    u.Email.Contains(request.Q) ||
                    (u.Role != null && u.Role.RoleName.Contains(request.Q))
                );
            }

            var dtoQuery = query.Select(u => new UserDTO
            {
                UserID = u.UserID,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                Phone = u.Phone,
                DateOfBirth = u.DateOfBirth,
                ProfileImage = u.ProfileImage,
                RoleName = u.Role != null ? u.Role.RoleName : null,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            });

            return await MyPagedList<UserDTO>.CreateAsync(dtoQuery, request, cancellationToken);
        }

        public class UserGetAllRequest : MyPagedRequest
        {
            public string? Q { get; set; } = string.Empty;
        }
    }
}
