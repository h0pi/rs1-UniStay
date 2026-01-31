using Microsoft.AspNetCore.Mvc;
using Stripe;
using UniStay.API.Data.Models.Dto.Stripe;
using UniStay.API.Data;
using UniStay.API.Helper.Api;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.StripeEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreatePaymentIntentEndpoint(
    ApplicationDbContext db)
    : MyEndpointBaseAsync
        .WithRequest<CreatePaymentIntentDTO>
        .WithActionResult<PaymentIntentResponseDTO>
    {
        [HttpPost("create-intent")]
        public override async Task<ActionResult<PaymentIntentResponseDTO>> HandleAsync(
            [FromBody] CreatePaymentIntentDTO request, CancellationToken cancellation = default)
        {
            var invoice = await db.Invoice
                .FirstOrDefaultAsync(x => x.InvoiceID == request.InvoiceId);

            if (invoice == null)
                return BadRequest(new { message = "Invoice not found" });

            if (invoice.Paid)
                return BadRequest(new { message = "Invoice already paid" });

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(invoice.TotalAmount * 100),
                Currency = "bam",
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "invoiceId", invoice.InvoiceID.ToString() },
                    { "studentId", invoice.StudentID.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return Ok(new PaymentIntentResponseDTO
            {
                ClientSecret = intent.ClientSecret,
                Amount = invoice.TotalAmount
            });
        }
    }
}
