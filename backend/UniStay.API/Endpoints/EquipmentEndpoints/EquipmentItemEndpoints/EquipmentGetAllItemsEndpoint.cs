using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;

[ApiController]
[Route("api/equipment-items")]
public class EquipmentGetAllItemsEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public EquipmentGetAllItemsEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }

    // 1) GET ALL ITEMS BY EQUIPMENT ID
    [HttpGet("by-equipment/{equipmentId}")]
    public async Task<IActionResult> GetItems(int equipmentId)
    {
        var items = await _db.EquipmentRecord
            .Where(x => x.EquipmentID == equipmentId)
            .OrderByDescending(x=>x.IsAvailable)
            .Select(x => new
            {
                x.RecordID,
                x.SerialNumber,
                x.IsAvailable ,
                x.Location,
                x.AssignedAt,
                x.ReturnedAt
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        _db.EquipmentRecord.Remove(item);
        await _db.SaveChangesAsync();

        return Ok();
    }

}