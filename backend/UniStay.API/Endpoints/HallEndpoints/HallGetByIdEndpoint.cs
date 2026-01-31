using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.API.Data;
using UniStay.API.Dto.Hall;

namespace UniStay.API.Endpoints.Hall
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallGetByIdEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallGetByIdEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetById(int id)
        {
            var hall = await _context.Hall
                .Where(h => h.HallID == id)
                .Select(h => new HallDto
                {
                    HallID = h.HallID,
                    Name = h.Name,
                    Capacity = h.Capacity,
                    Description = h.Description,
                    AvailableFrom = h.AvailableFrom,
                    AvailableTo = h.AvailableTo,
                    IsAvailable = h.IsAvailable
                })
                .FirstOrDefaultAsync();

            if (hall == null)
                return NotFound("Hall not found.");

            return Ok(hall);
        }
    }
}