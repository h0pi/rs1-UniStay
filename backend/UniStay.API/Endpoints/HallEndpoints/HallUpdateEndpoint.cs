using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.API.Data;
using UniStay.API.Dto.Hall;

namespace UniStay.API.Endpoints.Hall
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallUpdateEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallUpdateEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HallUpdateDto dto)
        {
            var hall = await _context.Hall.FindAsync(id);
            if (hall == null)
                return NotFound();

            hall.Name = dto.Name;
            hall.Capacity = dto.Capacity;
            hall.Description = dto.Description;
            hall.AvailableFrom = dto.AvailableFrom;
            hall.AvailableTo = dto.AvailableTo;
            hall.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();
            return Ok(hall);
        }
    }
}