using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using UniStay.API.Data.Models.Dto.Invoice;

namespace UniStay.API.Endpoints.InvoiceEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateInvoiceEndpoint(
     ApplicationDbContext db)
     : MyEndpointBaseAsync
         .WithRequest<InvoiceCreateDTO>
         .WithActionResult<int>
    {
        [HttpPost("create")]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] InvoiceCreateDTO request,
            CancellationToken cancellationToken = default)
        {
            var invoice = new Invoices
            {
                StudentID = request.StudentId,
                TotalAmount = request.Amount,
                IssuedAt = true,
                Paid = false,
                EmailSent = false
            };

            db.Invoice.Add(invoice);
            await db.SaveChangesAsync(cancellationToken);

            return Ok(invoice.InvoiceID);
        }
    }
}
