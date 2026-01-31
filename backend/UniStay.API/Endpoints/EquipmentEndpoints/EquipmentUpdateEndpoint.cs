// EquipmentUpdateEndpoint.cs
using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Equipment;
using UniStay.API.Data;

[ApiController]
[Route("api/[controller]")]
public class EquipmentUpdateEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public EquipmentUpdateEndpoint(ApplicationDbContext db) { _db = db; }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EquipmentUpdateDto dto)
    {
        var model = await _db.Equipment.FindAsync(id);
        if (model == null) return NotFound("Not found");

        model.Quantity = dto.Quantity;
        model.AvailableQuantity = dto.AvailableQuantity;
        model.RentalPrice = dto.RentalPrice;

        _db.Equipment.Update(model);
        await _db.SaveChangesAsync();
        return Ok(model);
    }
}