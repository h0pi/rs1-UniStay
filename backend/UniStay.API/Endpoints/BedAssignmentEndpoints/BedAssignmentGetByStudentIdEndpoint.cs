using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.BedAssignment;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.BedAssignmentEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedAssignmentGetByStudentEndpoint(ApplicationDbContext db)
     : MyEndpointBaseAsync
         .WithRequest<int>
         .WithActionResult<BedAssignmentStudentDTO?>
    {
        [HttpGet("get-by-student/{studentId:int}")]
        public override async Task<ActionResult<BedAssignmentStudentDTO?>> HandleAsync(
            [FromRoute] int studentId,
            CancellationToken cancellationToken = default)
        {
            var assignment = await db.BedAssignment
                .Include(a => a.Bed)
                .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync(a => a.StudentID == studentId, cancellationToken);

            if (assignment == null)
                return NotFound(new { message = "This student has no assigned bed." });

            var dto = new BedAssignmentStudentDTO
            {
                AssignmentID = assignment.AssignmentID,
                BedID = assignment.BedID,
                BedNumber = assignment.Bed.BedNumber,
                RoomID = assignment.Bed.RoomID,
                RoomNumber = assignment.Bed.Room.RoomNumber,
                Floor = assignment.Bed.Room.Floor
            };

            return Ok(dto);
        }
    }

}
