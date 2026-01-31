using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Data.Models.Dto.Fault;

namespace UniStay.API.Endpoints.Fault
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaultCreateEndpoint : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FaultCreateEndpoint(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FaultCreateDto dto)
        {
            var userIdHeader = Request.Headers["X-User-Id"].FirstOrDefault();
            var userId = string.IsNullOrEmpty(userIdHeader)?1:int.Parse(userIdHeader);
            var fault = new Faults
            {
                Title = dto.Title,
                Description = dto.Description,
                ReportedByUserID = userId,
                ReportedAt = DateTime.UtcNow,
                IsResolved = false,
                Status="Open",
                RoomID=dto.RoomID
            };

            _context.Fault.Add(fault);
            await _context.SaveChangesAsync();

            return Ok(fault);
        }
    }
}