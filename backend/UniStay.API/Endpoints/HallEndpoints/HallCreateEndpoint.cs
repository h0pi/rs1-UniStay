using Microsoft.AspNetCore.Mvc;
using System;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Dto.Hall;

namespace UniStay.API.Endpoints.Hall
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallCreateEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallCreateEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HallCreateDto dto)
        {
            var hall = new Halls
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Description = dto.Description,
                AvailableFrom = dto.AvailableFrom,
                AvailableTo = dto.AvailableTo,
                IsAvailable = dto.IsAvailable
            };

            _context.Hall.Add(hall);
            await _context.SaveChangesAsync();

            return Ok(hall);
        }
    }
}