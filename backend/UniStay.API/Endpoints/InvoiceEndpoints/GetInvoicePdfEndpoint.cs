using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data.Models;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Helper.Pdf;

namespace UniStay.API.Endpoints.InvoiceEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetInvoicePdfEndpoint(
        ApplicationDbContext db)
        : MyEndpointBaseAsync
            .WithRequest<int>
            .WithActionResult
    {
        [HttpGet("{invoiceId}/pdf")]
        public override async Task<ActionResult> HandleAsync(
            [FromRoute] int invoiceId,
            CancellationToken cancellationToken = default)
        {
            var invoice = await db.Invoice
                .FirstOrDefaultAsync(x => x.InvoiceID == invoiceId, cancellationToken);

            if (invoice == null)
                return NotFound("Invoice not found.");

            if (!invoice.Paid)
                return BadRequest("Invoice is not paid.");

            byte[] pdfBytes = InvoicePdfGenerator.Generate(invoice);


            return File(
                pdfBytes,
                "application/pdf",
                $"invoice-{invoice.InvoiceID}.pdf"
            );
        }

    }
}
