using Microsoft.AspNetCore.Mvc;
using System.Text;
using UniStay.API.Data.Models.Dto.User;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace UniStay.API.Endpoints.UserEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserUpdateEndpoint(ApplicationDbContext db)
     : MyEndpointBaseAsync
         .WithRequest<UserUpdateDTO>
         .WithActionResult<int>
    {
        [HttpPut]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] UserUpdateDTO request,
            CancellationToken cancellationToken = default)
        {
            // 1️⃣ Pronađi usera
            var user = await db.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == request.UserID, cancellationToken);

            if (user == null)
                return NotFound($"User with ID {request.UserID} not found.");

            // If RoleID is sent -> check if given id exists 
            if (request.RoleID.HasValue)
            {
                bool roleExists = await db.Role.AnyAsync(r => r.RoleID == request.RoleID.Value, cancellationToken);
                if (!roleExists)
                    return BadRequest("Role with the given ID does not exist.");
            }

            if (request.Email != null)
                user.Email = request.Email;

            if (request.FirstName != null)
                user.FirstName = request.FirstName;

            if (request.LastName != null)
                user.LastName = request.LastName;

            if (request.Phone != null)
                user.Phone = request.Phone;

            if (request.DateOfBirth.HasValue)
                user.DateOfBirth = request.DateOfBirth.Value;

            if (request.Username != null)
                user.Username = request.Username;

            if (request.ProfileImage != null)
                user.ProfileImage = request.ProfileImage;

            if (request.RoleID.HasValue)
                user.RoleID = request.RoleID.Value;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                using var sha = SHA256.Create();
                user.PasswordHash = Convert.ToBase64String(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                );
            }

            user.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(cancellationToken);

            return Ok(user.UserID);
        }
    }
}
