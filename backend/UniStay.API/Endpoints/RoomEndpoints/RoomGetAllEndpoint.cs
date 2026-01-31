using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models.Dto.Room;
using UniStay.API.Helper;
using UniStay.API.Helper.Api;
using static UniStay.API.Endpoints.RoomEndpoints.RoomGetAllEndpoint;

namespace UniStay.API.Endpoints.RoomEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<RoomGetAllRequest>
        .WithResult<MyPagedList<RoomDTO>>
    {
        [HttpGet("filter")]
        public override async Task<MyPagedList<RoomDTO>> HandleAsync(
            [FromQuery] RoomGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = db.Room
            .Include(r => r.Beds)
                .ThenInclude(b => b.BedAssignments)
                    .ThenInclude(ba => ba.Student)
            .AsQueryable();

            //  SEARCH
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(r =>
                    r.RoomNumber.Contains(request.Q) ||
                    r.Description.Contains(request.Q)
                );
            }

            
            var dtoQuery = query.Select(r => new RoomDTO
            {
                RoomID = r.RoomID,
                RoomNumber = r.RoomNumber,
                Floor = r.Floor,
                MaxOccupancy = r.MaxOccupancy,
                Description = r.Description,

                Students = r.Beds
                    .SelectMany(b => b.BedAssignments)
                    .Where(ba => ba.Student != null)
                    .Select(ba => new RoomUserDTO
                    {
                        UserId = ba.Student.UserID,
                        Name = ba.Student.FirstName,
                        LastName = ba.Student.LastName
                    })
                    .ToList()
            });
            return await MyPagedList<RoomDTO>.CreateAsync(dtoQuery, request, cancellationToken);
        }
        public class RoomGetAllRequest : MyPagedRequest
        {
            public string? Q { get; set; } = string.Empty;
        }
    }
}
