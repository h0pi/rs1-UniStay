using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models.Dto.Invoice;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace UniStay.API.Endpoints.InvoiceEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class GetInvoicesByUserIdEndpoint(
        ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<int>
            .WithActionResult<List<InvoiceGetDTO>>
    {
        [HttpGet("{studentId}")]
        public override async Task<ActionResult<List<InvoiceGetDTO>>> HandleAsync(
            [FromRoute] int studentId,
            CancellationToken cancellationToken = default)
        {
            var invoices = await db.Invoice
                .Where(i => i.StudentID == studentId)
                .Select(i => new InvoiceGetDTO
                {
                    InvoiceID = i.InvoiceID,
                    TotalAmount = i.TotalAmount,
                    Paid = i.Paid,
                    IssuedAt = i.IssuedAt
                })
                .ToListAsync(cancellationToken);

            return Ok(invoices);
        }
    }
}
