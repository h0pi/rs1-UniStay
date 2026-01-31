using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;

[ApiController]
[Route("api/equipment-items")]
public class EquipmentGetSignleItemsEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public EquipmentGetSignleItemsEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }


    // 2) GET SINGLE ITEM
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _db.EquipmentRecord.FindAsync(id);
        if (item == null) return NotFound();

        return Ok(item);
    }

 
}