// EquipmentCreateEndpoint.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Equipment;

[ApiController]
[Route("api/[controller]")]
public class EquipmentCreateEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public EquipmentCreateEndpoint(ApplicationDbContext db) { _db = db; }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquipmentCreateDto dto)
    {
        var model = new Equipments
        {
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            AvailableQuantity = dto.AvailableQuantity,
            RentalPrice = dto.RentalPrice,
            EquipmentType = dto.EquipmentType
        };

        _db.Equipment.Add(model);
        await _db.SaveChangesAsync();

        for (int i = 1; i <= model.Quantity; i++)
        {
            _db.EquipmentRecord.Add(new EquipmentRecords
            {
                EquipmentID = model.EquipmentID,
                SerialNumber = $"{model.Name}-{i}",
                IsAvailable = true
            });
        }

        await _db.SaveChangesAsync();

        return Ok(model);
    }
}