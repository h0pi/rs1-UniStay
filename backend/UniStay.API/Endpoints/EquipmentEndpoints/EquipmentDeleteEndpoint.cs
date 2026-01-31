
using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;

[ApiController]
[Route("api/[controller]")]
public class EquipmentDeleteEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public EquipmentDeleteEndpoint(ApplicationDbContext db) { _db = db; }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _db.Equipment.FindAsync(id);
        if (model == null) return NotFound("Not found");

        _db.Equipment.Remove(model);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Deleted", deletedId = id });
    }
}