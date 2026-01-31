using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Equipment;
using UniStay.API.Data;

[ApiController]
[Route("api/[controller]")]




    public class EquipmentGetOneEndpoint:ControllerBase
    {    private readonly ApplicationDbContext _db;
    public EquipmentGetOneEndpoint(ApplicationDbContext db) { _db = db; }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneEquipment(int id)
        {
            var eq = await _db.Equipment.FindAsync(id);
            if (eq == null) return NotFound();

            return Ok(eq);
        }
    }

