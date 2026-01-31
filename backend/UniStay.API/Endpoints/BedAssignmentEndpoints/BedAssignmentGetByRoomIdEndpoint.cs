using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.BedAssignment;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.BedAssignmentEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedAssignmentGetByRoomIdEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<List<BedAssignmentRoomDTO>>
    {
        [HttpGet("get-by-room/{roomId:int}")]
        public override async Task<List<BedAssignmentRoomDTO>> HandleAsync(
            [FromRoute] int roomId,
            CancellationToken cancellationToken = default)
        {
            var list = await db.BedAssignment
                .Include(a => a.Bed)
                .Include(a => a.Student)
                .Where(a => a.Bed.RoomID == roomId)
                .Select(a => new BedAssignmentRoomDTO
                {
                    AssignmentID = a.AssignmentID,
                    BedID = a.BedID,
                    BedNumber = a.Bed.BedNumber,
                    StudentID = a.StudentID,
                    Name = a.Student.FirstName,
                    LastName = a.Student.LastName
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }

}
