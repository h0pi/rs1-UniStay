using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Fault;

namespace UniStay.API.Endpoints.Fault;

[ApiController]
[Route("api/[controller]")]
public class FaultUpdateEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public FaultUpdateEndpoint(ApplicationDbContext db) { _db = db; }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FaultUpdateDto dto)
    {
        var model = await _db.Fault.FindAsync(id);
        if (model == null) return NotFound();

        model.Title = dto.Title;
        model.Description = dto.Description;
        model.IsResolved = dto.ResolvedAt.HasValue;
        model.ResolvedAt = dto.ResolvedAt;
        model.Status = dto.Status;
        model.Priority = dto.Priority;
        model.ResolvedAt = dto.ResolvedAt;

        _db.Fault.Update(model);
        await _db.SaveChangesAsync();
        return Ok(model);
    }

}
