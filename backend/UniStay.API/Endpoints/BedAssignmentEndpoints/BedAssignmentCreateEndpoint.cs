using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.BedAssignment;
using UniStay.API.Data.Models;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.BedAssignmentEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedAssignEndpoint(ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<BedAssignmentCreateDTO>
            .WithActionResult<object>
    {
        [HttpPost("assign")]
        public override async Task<ActionResult<object>> HandleAsync(
            [FromBody] BedAssignmentCreateDTO request,
            CancellationToken cancellationToken = default)
        {
            // validations
            if (request.BedID <= 0 || request.StudentID <= 0)
                return BadRequest(new { message = "BedID and StudentID are required." });

            if (request.FromDate >= request.ToDate)
                return BadRequest(new { message = "FromDate must be before ToDate." });

            // existence off beds and students check
            var bed = await db.Bed.FirstOrDefaultAsync(b => b.BedID == request.BedID, cancellationToken);
            if (bed == null)
                return NotFound(new { message = "Bed does not exist." });

            var student = await db.User.FirstOrDefaultAsync(s => s.UserID == request.StudentID, cancellationToken);
            if (student == null)
                return NotFound(new { message = "Student does not exist." });

            // check if bed is already taken
            bool bedTaken = await db.BedAssignment.AnyAsync(a =>
                a.BedID == request.BedID &&
                a.ToDate > request.FromDate &&
                a.FromDate < request.ToDate,
                cancellationToken);

            if (bedTaken)
                return BadRequest(new { message = "Bed is already assigned in the selected date range." });

            // check if student already has bed
            bool studentHasBed = await db.BedAssignment.AnyAsync(a =>
                a.StudentID == request.StudentID &&
                a.ToDate > request.FromDate &&
                a.FromDate < request.ToDate,
                cancellationToken);

            if (studentHasBed)
                return BadRequest(new { message = "Student already has a bed assigned for this period." });

            // assign bed to student 
            var assignment = new BedAssignments
            {
                BedID = request.BedID,
                StudentID = request.StudentID,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            db.BedAssignment.Add(assignment);
            await db.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "Bed successfully assigned to student!" });
        }
    } 
}
