using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.BedAssignment;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using UniStay.API.Helper;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.BedAssignmentEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedAssignmentGetAllEndpoint(ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<BedAssignmentGetAllRequest>
            .WithResult<MyPagedList<BedAssignmentDTO>>
    {
        [HttpGet("all")]
        public override async Task<MyPagedList<BedAssignmentDTO>> HandleAsync(
            [FromQuery] BedAssignmentGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = db.BedAssignment
                .Include(a => a.Bed)
                .ThenInclude(b => b.Room)
                .Include(a => a.Student)
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(a =>
                    a.Student.FirstName.Contains(request.Q) ||
                    a.Student.LastName.Contains(request.Q) ||
                    a.Bed.BedNumber.Contains(request.Q) ||
                    a.Bed.Room.RoomNumber.Contains(request.Q));
            }

            
            var dtoQuery = query.Select(a => new BedAssignmentDTO
            {
                AssignmentID = a.AssignmentID,

                BedID = a.BedID,
                BedNumber = a.Bed.BedNumber,

                RoomID = a.Bed.RoomID,
                RoomNumber = a.Bed.Room.RoomNumber,

                StudentID = a.StudentID,
                StudentName = a.Student.FirstName,
                StudenLName = a.Student.LastName
            });

            
            return await MyPagedList<BedAssignmentDTO>.CreateAsync(dtoQuery, request, cancellationToken);
        }

    }

    public class BedAssignmentGetAllRequest : MyPagedRequest
    {
        public string? Q { get; set; } =string.Empty;
    }
}