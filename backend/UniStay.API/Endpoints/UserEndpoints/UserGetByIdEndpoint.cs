using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models.Dto.User;
using UniStay.API.Helper.Api;
using UniStay.API.Services;

namespace UniStay.API.Endpoints.UserEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    //[MyAuthorization("Admin")] // možeš promijeniti u "Employee" ili maknuti
    public class UserGetByIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
     .WithRequest<int>
     .WithActionResult<UserDTO>
    {
        [HttpGet("{id:int}")]
        public override async Task<ActionResult<UserDTO>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            
            var user = await db.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == id, cancellationToken);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            var dto = new UserDTO
            {
                UserID = user.UserID,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                ProfileImage = user.ProfileImage,

                RoleName = user.Role?.RoleName,

                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(dto);
        }
    }
}
