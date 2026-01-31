using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.API.Data;
using UniStay.API.Dto.Hall;

namespace UniStay.API.Endpoints.Hall
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallGetAllEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallGetAllEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetAll(
            [FromQuery] string? name,
            [FromQuery] int? minCapacity,
            [FromQuery] int? maxCapacity,
            [FromQuery] bool? isAvailable,
            [FromQuery] DateTime? date)
        {
            var query = _context.Hall.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(h => h.Name.Contains(name));

            if (minCapacity.HasValue)
                query = query.Where(h => h.Capacity >= minCapacity.Value);

            if (maxCapacity.HasValue)
                query = query.Where(h => h.Capacity <= maxCapacity.Value);

            if (isAvailable.HasValue)
                query = query.Where(h => h.IsAvailable == isAvailable.Value);

            if (date.HasValue)
                query = query.Where(h => h.AvailableFrom <= date && h.AvailableTo >= date);

            var halls = await query.Select(h => new HallDto
            {
                HallID = h.HallID,
                Name = h.Name,
                Capacity = h.Capacity,
                Description = h.Description,
                AvailableFrom = h.AvailableFrom,
                AvailableTo = h.AvailableTo,
                IsAvailable = h.IsAvailable
            }).ToListAsync();

            return Ok(halls);
        }
    }
}