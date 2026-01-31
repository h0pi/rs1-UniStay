using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models.Dto.Room;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.RoomEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomGetByIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
            .WithRequest<int>
            .WithActionResult<object>
    {
        [HttpGet]
        public override async Task<ActionResult<object>> HandleAsync(
            [FromQuery] int request,
            CancellationToken cancellationToken = default)
        {
            var room = await db.Room
            .Include(r => r.Beds)
                .ThenInclude(b => b.BedAssignments)
                    .ThenInclude(ba => ba.Student)
            .FirstOrDefaultAsync(r => r.RoomID == request, cancellationToken);
            if (room == null)
                return NotFound(new { message = "Room not found." });
            var dto = new RoomDTO
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber,
                Floor = room.Floor,
                MaxOccupancy = room.MaxOccupancy,
                Description = room.Description,
                Students = room.Beds
                .SelectMany(b => b.BedAssignments)
                .Where(ba => ba.Student != null)
                .Select(ba => new RoomUserDTO
                {
                    UserId = ba.Student.UserID,
                    Name = ba.Student.FirstName,
                    LastName = ba.Student.LastName

                })
                .ToList()
            };
            return Ok(dto);
        }
    }
}
