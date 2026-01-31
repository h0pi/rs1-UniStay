using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Bed;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Helper;

namespace UniStay.API.Endpoints.BedEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedGetAllFreeEndpoint(ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<BedGetAllFreeRequest>
            .WithActionResult<MyPagedList<BedFreeDTO>>
    {
        [HttpGet("free")]
        public override async Task<ActionResult<MyPagedList<BedFreeDTO>>> HandleAsync(
            [FromQuery] BedGetAllFreeRequest request,
            CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;

            var freeBeds = await db.Bed
                .Include(b => b.Room)
                .Where(b => !db.BedAssignment.Any(a => a.BedID == b.BedID))
                .Select(b => new BedFreeDTO
                {
                    BedId = b.BedID,
                    BedNumber = b.BedNumber,
                    RoomId = b.RoomID,
                    RoomNumber = b.Room.RoomNumber,
                    Floor = b.Room.Floor
                })
                .ToListAsync(cancellationToken);

            return Ok(freeBeds);
        }
    }

    public class BedGetAllFreeRequest : MyPagedRequest
    {
        public string? Q { get; set; } = string.Empty;
    }
}

