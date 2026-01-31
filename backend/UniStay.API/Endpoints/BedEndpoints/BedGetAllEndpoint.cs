using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models.Dto.Bed;
using UniStay.API.Helper;
using UniStay.API.Helper.Api;

namespace UniStay.API.Endpoints.BedEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<BedGetAllRequest>
        .WithResult<MyPagedList<BedDTO>>
    {
        [HttpGet]
        public override async Task<MyPagedList<BedDTO>> HandleAsync(
            [FromQuery] BedGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = db.Bed
            .Include(b => b.Room)
            .AsQueryable();

            // 2. PRETRAGA (Q)
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(b =>
                    b.BedNumber.Contains(request.Q) ||
                    b.Room.RoomNumber.Contains(request.Q));
            }

            // 3. Mapiranje u DTO
            var dtoQuery = query.Select(b => new BedDTO
            {
                BedId = b.BedID,
                BedNumber = b.BedNumber,
                RoomID = b.RoomID,
                RoomNumber = b.Room.RoomNumber
            });

            // 4. Paginacija
            return await MyPagedList<BedDTO>.CreateAsync(dtoQuery, request, cancellationToken);

        }
    }

    public class BedGetAllRequest : MyPagedRequest
    {
        public string? Q { get; set; } = string.Empty;
    }
}
