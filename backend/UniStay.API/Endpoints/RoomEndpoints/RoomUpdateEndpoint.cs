using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Room;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.RoomEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomUpdateEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
        .WithRequest<RoomUpdateDTO>
        .WithActionResult<object>
    {
        [HttpPut]
        public override async Task<ActionResult<object>> HandleAsync(
            [FromBody] RoomUpdateDTO request,
            CancellationToken cancellationToken = default)
        {
            var room = await db.Room.FirstOrDefaultAsync(r => r.RoomID == request.RoomID);

            if (room == null)
                return NotFound(new { message = $"Room with ID {request.RoomID} not found." });

            if (request.Description != null)
                room.Description = request.Description;

            if (request.Floor.HasValue)
                room.Floor = request.Floor.Value;

            if (request.MaxOccupancy.HasValue)
                room.MaxOccupancy = request.MaxOccupancy.Value;

            await db.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "Room updated successfully." });
        }
    }
}
