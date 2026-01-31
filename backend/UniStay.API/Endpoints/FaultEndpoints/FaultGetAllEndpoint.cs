using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models.Dto.Fault;

namespace UniStay.API.Endpoints.Fault
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaultGetAllEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FaultGetAllEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaultListDto>>> GetAll(
            [FromQuery] string? title,
            [FromQuery] int? reportedBy,
            [FromQuery] bool? isResolved,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to
        )
        {
            var query = _context.Fault.AsQueryable();

            // ------- FILTERI -------
            if (!string.IsNullOrEmpty(title))
                query = query.Where(f => f.Title.Contains(title));

            if (reportedBy.HasValue)
                query = query.Where(f => f.ReportedByUserID == reportedBy.Value);

            if (isResolved.HasValue)
                query = query.Where(f => f.IsResolved == isResolved.Value);

            if (from.HasValue)
                query = query.Where(f => f.ReportedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(f => f.ReportedAt <= to.Value);

            // ------- SELECT DTO -------
            var faults = await query
                .OrderByDescending(f => f.ReportedAt)
                .Select(f => new FaultListDto
                {
                    FaultID = f.FaultID,
                    Title = f.Title,
                    Description = f.Description,
                    ReportedByUserID = f.ReportedByUserID,
                    ReportedByUserName = f.ReportedByUser.FirstName + " " + f.ReportedByUser.LastName,
                    IsResolved = f.IsResolved ?? false,
                    ReportedAt = f.ReportedAt,
                    ResolvedAt = f.ResolvedAt,
                    Priority = f.Priority,
                    RoomID = f.RoomID
                })
                .ToListAsync();

            return Ok(faults);
        }
    }
}