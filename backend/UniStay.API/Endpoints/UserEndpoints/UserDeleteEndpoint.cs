using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.UserEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDeleteEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<bool>
    {
        [HttpDelete("{id:int}")]
        public override async Task<ActionResult<bool>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var user = await db.User
                .FirstOrDefaultAsync(u => u.UserID == id, cancellationToken);

            if (user == null)
                return NotFound($"User with ID {id} does not exist.");

            bool isAssigned = await db.BedAssignment
            .AnyAsync(a => a.StudentID == id, cancellationToken);

            if (isAssigned)
                return BadRequest(new { message = "User is assigned to a bed and cannot be deleted. Unassign the student first." });

            db.User.Remove(user);

            await db.SaveChangesAsync(cancellationToken);

            return Ok(true);
        }
    }
}
