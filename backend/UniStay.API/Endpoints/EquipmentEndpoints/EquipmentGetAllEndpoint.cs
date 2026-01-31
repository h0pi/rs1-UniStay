// EquipmentGetAllEndpoint.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data.Models.Dto.Equipment;
using UniStay.API.Data;

[ApiController]
[Route("api/[controller]")]
public class EquipmentGetAllEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public EquipmentGetAllEndpoint(ApplicationDbContext db) { _db = db; }

    // supports query filters: name, type, minQty, maxQty, availableOnly
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] int? minQty,
        [FromQuery] int? maxQty,
        [FromQuery] bool? availableOnly)
    {
        var q = _db.Equipment.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            q = q.Where(e => e.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(type))
            q = q.Where(e => e.EquipmentType == type);

        if (minQty.HasValue)
            q = q.Where(e => e.Quantity >= minQty.Value);

        if (maxQty.HasValue)
            q = q.Where(e => e.Quantity <= maxQty.Value);

        if (availableOnly.HasValue && availableOnly.Value)
            q = q.Where(e => e.AvailableQuantity > 0);

        var list = await q.Select(e => new EquipmentDto
        {
            EquipmentID = e.EquipmentID,
            Name = e.Name,
            Description = e.Description,
            RentalPrice = e.RentalPrice,
            EquipmentType = e.EquipmentType,

            Quantity=_db.EquipmentRecord.Count(r=>r.EquipmentID==e.EquipmentID),
            AvailableQuantity=_db.EquipmentRecord.Count(r=>r.EquipmentID==e.EquipmentID && r.IsAvailable)
        }).ToListAsync();

        return Ok(list);
    }
}