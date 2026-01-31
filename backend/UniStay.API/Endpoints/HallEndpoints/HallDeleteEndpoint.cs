using Microsoft.AspNetCore.Mvc;
using System;
using UniStay.API.Data;

namespace UniStay.API.Endpoints.Hall
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallDeleteEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallDeleteEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hall = await _context.Hall.FindAsync(id);
            if (hall == null)
                return NotFound("Hall not found.");

            _context.Hall.Remove(hall);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Hall has been successfully deleted.", deletedId = id });
        }
    }
}