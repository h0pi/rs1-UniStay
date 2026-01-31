using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Fault;

[ApiController]
[Route("api/faults")]
public class FaultFilterEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public FaultFilterEndpoint(ApplicationDbContext db) { _db = db; }


    [HttpPost("filter")]
    public async Task<IActionResult> Filter([FromBody] FaultFilterDto filter)
    {
        var q = _db.Fault.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            q = q.Where(x => x.Title.Contains(filter.Title));

        if (filter.ReportedBy.HasValue)
            q = q.Where(x => x.ReportedByUserID == filter.ReportedBy.Value);

        if (filter.IsResolved.HasValue)
            q = q.Where(x => x.IsResolved == filter.IsResolved.Value);

        if (filter.From.HasValue)
            q = q.Where(x => x.ReportedAt >= filter.From.Value);

        if (filter.To.HasValue)
            q = q.Where(x => x.ReportedAt <= filter.To.Value);

        var outList = await q.OrderByDescending(x => x.ReportedAt).ToListAsync();
        return Ok(outList);
    }
}
