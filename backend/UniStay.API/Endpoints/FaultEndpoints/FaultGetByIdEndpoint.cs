using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Fault;

[ApiController]
[Route("api/faults")]
public class FaultGetByIdEndpoint:ControllerBase
{
    private readonly ApplicationDbContext _db;
    public FaultGetByIdEndpoint(ApplicationDbContext db) { _db = db; }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
       var item = await _db.Fault.FindAsync(id);
       if (item == null) return NotFound();
       return Ok(item);
    }
}
