using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;

[ApiController]
[Route("api/equipment-items")]
public class EquipmentRecordCreateEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public EquipmentRecordCreateEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("create/{equipmentId}")]
    public async Task<IActionResult> Create(int equipmentId,[FromBody] EquipmentRecordCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newItem = new EquipmentRecords
        {
            EquipmentID = equipmentId,
            SerialNumber = dto.SerialNumber,
            //IsAvailable = dto.IsAvailable,

            // nov item nema ove podatke
            AssignedAt = null,
            ReturnedAt = null,
            Location = null,

            // StudentID i EmployeeID možeš staviti 0 ili null ako su nullable
            StudentID = null,
            EmployeeID = null
        };

        _db.EquipmentRecord.Add(newItem);
        await _db.SaveChangesAsync();

        return Ok(newItem);
    }
}

public class EquipmentRecordCreateDto
{
   // public int EquipmentId { get; set; }
    public string SerialNumber { get; set; }

}