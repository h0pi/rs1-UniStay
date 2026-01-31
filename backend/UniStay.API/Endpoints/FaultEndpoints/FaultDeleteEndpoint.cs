using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Fault;

[ApiController]
[Route("api/faults")]
public class FaultDeleteEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public FaultDeleteEndpoint(ApplicationDbContext db) { _db = db; }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _db.Fault.FindAsync(id);
        if (model == null) return NotFound("Not found");

        _db.Fault.Remove(model);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Fault has been successfully deleted.", deletedId = id } );
    }

}
