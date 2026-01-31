using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;

[ApiController]
[Route("api/equipment-records")]
public class EquipmentItemsUpdateEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public EquipmentItemsUpdateEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }

    // 🚀 1) GET ALL RECORDS FOR ONE EQUIPMENT TYPE
    [HttpGet("by-equipment/{equipmentId}")]
    public async Task<IActionResult> GetByEquipment(int equipmentId)
    {
        var items = await _db.EquipmentRecord
            .Where(x => x.EquipmentID == equipmentId)
            .ToListAsync();

        return Ok(items);
    }

    // 🚀 2) GET ONE RECORD
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        return Ok(item);
    }

    // 🚀 3) UPDATE BASIC DATA (serial, availability)
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] EquipmentRecords dto)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        item.SerialNumber = dto.SerialNumber;
        item.IsAvailable = dto.IsAvailable;

        await _db.SaveChangesAsync();
        return Ok(item);
    }

    // 🚀 4) UPDATE ONLY AVAILABILITY (simple toggle)
    [HttpPut("{id}/availability")]
    public async Task<IActionResult> UpdateAvailability(int id, [FromQuery] bool isAvailable)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        item.IsAvailable = isAvailable;

        // ako se oslobađa → obriši datume
        if (isAvailable)
        {
            item.AssignedAt = null;
            item.ReturnedAt = null;
            item.Location = null;
        }

        await _db.SaveChangesAsync();
        return Ok(item);
    }

    // 🚀 5) ASSIGN = MARK AS TAKEN
    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignDto dto)
    {
        var item = await _db.EquipmentRecord.FindAsync(dto.EquipmentRecordID);
        if (item == null) return NotFound();

        item.IsAvailable = false;
        item.AssignedAt = dto.AssignedAt;
        item.ReturnedAt = dto.ReturnedAt;
        item.Location = dto.Location;

        await _db.SaveChangesAsync();
        return Ok(item);
    }

    // 🚀 6) RELEASE = MAKE AVAILABLE
    [HttpPost("release/{id}")]
    public async Task<IActionResult> ReleaseItem(int id)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        item.IsAvailable = true;
        item.AssignedAt = null;
        item.ReturnedAt = null;
        item.Location = null;

        await _db.SaveChangesAsync();
        return Ok(item);
    }
}

public class AssignDto
{
    public int EquipmentRecordID { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string? Location { get; set; }
}