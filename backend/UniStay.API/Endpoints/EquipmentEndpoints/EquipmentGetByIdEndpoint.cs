// EquipmentGetByIdEndpoint.cs
using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Equipment;
using UniStay.API.Data;

[ApiController]
[Route("api/[controller]")]
public class EquipmentGetByIdEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public EquipmentGetByIdEndpoint(ApplicationDbContext db) { _db = db; }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var e = await _db.Equipment.FindAsync(id);
        if (e == null) return NotFound("Equipment not found.");
        var dto = new EquipmentDto
        {
            EquipmentID = e.EquipmentID,
            Name = e.Name,
            Description = e.Description,
            Quantity = e.Quantity,
            AvailableQuantity = e.AvailableQuantity,
            RentalPrice = e.RentalPrice,
            EquipmentType = e.EquipmentType
        };
        return Ok(dto);
    }
}