using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.BedAssignmentEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedAssignmentDeleteEndpoint(ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<int>
            .WithActionResult<object>
    {
        [HttpDelete("{id:int}")]
        public override async Task<ActionResult<object>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var assignment = await db.BedAssignment
                .FirstOrDefaultAsync(a => a.AssignmentID == id, cancellationToken);

            if (assignment == null)
                return NotFound(new { message = "Assignment not found." });

            db.BedAssignment.Remove(assignment);
            await db.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "Bed assignment successfully removed." });
        }
    }
}
