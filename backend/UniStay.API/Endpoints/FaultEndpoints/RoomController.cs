using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;
using UniStay.API.Dto.Room;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RoomsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll(
        [FromQuery] string? roomNumber     
    )
    {
        var query = _db.Room.AsQueryable();

        if (!string.IsNullOrWhiteSpace(roomNumber))
            query = query.Where(r => r.RoomNumber.Contains(roomNumber));

        var rooms = await query.Select(r => new RoomDto
        {
            RoomID = r.RoomID,
            RoomNumber = r.RoomNumber
        }).ToListAsync();

        return Ok(rooms);
    }
}