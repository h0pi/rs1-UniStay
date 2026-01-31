using Microsoft.AspNetCore.Mvc;
using System.Text;
using UniStay.API.Data.Models.Dto.User;
using UniStay.API.Data.Models;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using UniStay.API.Services;
using System.Security.Cryptography;

namespace UniStay.API.Endpoints.UserEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    //[MyAuthorization("Admin")]  
    public class UserCreateEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
     .WithRequest<UserCreateDTO>
     .WithActionResult<string>
    {
        [HttpPost("create")]
        public override async Task<ActionResult<string>> HandleAsync(
            [FromBody] UserCreateDTO request,
            CancellationToken cancellationToken = default)
        {
            
            var existingEmail = db.User.FirstOrDefault(u => u.Email == request.Email);
            if (existingEmail != null)
                return BadRequest(new { message = "Email is already in use." });

            
            string finalUsername;

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                finalUsername = request.FirstName;
            }
            else
            {
                finalUsername = request.Username;
            }

                
            using var sha = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(request.Password);
            string passwordHash = Convert.ToBase64String(sha.ComputeHash(passwordBytes));

            //int finalRoleId = request.RoleID ?? 2;

            var newUser = new Users
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Username = finalUsername,
                PasswordHash = passwordHash,
                ProfileImage = request.ProfileImage ?? string.Empty,
                RoleID = request.RoleID,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.User.Add(newUser);
            await db.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "User Created!" });
        }
    }
}
