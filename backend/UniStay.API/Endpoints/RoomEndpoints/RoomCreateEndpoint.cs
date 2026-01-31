using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Room;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.RoomEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomCreateEndpoint(ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<RoomCreateDTO>
            .WithActionResult<string>
    {
        [HttpPost("create")]
        public override async Task<ActionResult<string>> HandleAsync(
            [FromBody] RoomCreateDTO request,
            CancellationToken cancellationToken = default)
        {
            // VALIDACIJE — dodaj po potrebi
            if (string.IsNullOrWhiteSpace(request.RoomNumber))
                return BadRequest(new { message = "Room number is required." });

            

            // PROVJERA DA LI POSTOJI ROOM S ISTIM BROJEM
            var exists = await db.Room.AnyAsync(r => r.RoomNumber == request.RoomNumber, cancellationToken);
            if (exists)
                return BadRequest(new { message = "A room with this number already exists." });

            // KREIRANJE ROOM ENTITETA
            var room = new Rooms
            {
                RoomNumber = request.RoomNumber,
                Floor = request.Floor,
                MaxOccupancy = request.MaxOccupancy,
                Description = request.Description
            };

            db.Room.Add(room);
            await db.SaveChangesAsync(cancellationToken);

            for (int i = 1; i <= request.MaxOccupancy; i++)
            {
                var bed = new Beds
                {
                    RoomID = room.RoomID,
                    BedNumber = $"{request.RoomNumber}-{i}",
                };
                db.Bed.Add(bed);
            }

            await db.SaveChangesAsync(cancellationToken);
            return Ok(new { message = "Room created successfully!" });
        }
    }
}
