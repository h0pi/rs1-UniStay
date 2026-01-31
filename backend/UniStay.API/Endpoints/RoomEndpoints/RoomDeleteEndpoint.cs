using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using UniStay.API.Data;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.RoomEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomDeleteEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<object>
    {
        [HttpDelete("{id:int}")]
        public override async Task<ActionResult<object>> HandleAsync(int id, CancellationToken ct)
        {
            var room = await db.Room
            .Include(r => r.Beds)
                .ThenInclude(b => b.BedAssignments)
            .FirstOrDefaultAsync(r => r.RoomID == id, ct);

            if (room == null)
                return NotFound(new { message = "Room not found." });

            bool hasAssignedStudents = room.Beds
             .Any(b => b.BedAssignments.Any());

            if (hasAssignedStudents)
            {
                return BadRequest(new
                {
                    message = "Cannot delete room because students are assigned to beds."
                });
            }

            
            if (room.Beds.Any())
            {
                // delete beds from the room 
                db.Bed.RemoveRange(room.Beds);
            }

            db.Room.Remove(room);
            await db.SaveChangesAsync(ct);

            return Ok(new { message = "Room deleted successfully." });
        }

    }
}
