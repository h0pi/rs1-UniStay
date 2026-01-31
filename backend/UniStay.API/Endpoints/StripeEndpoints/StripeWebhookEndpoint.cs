using Microsoft.AspNetCore.Mvc;
using Stripe;
using UniStay.API.Data.Models;
using UniStay.API.Data;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Endpoints.StripeEndpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public StripeWebhookController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var signature = Request.Headers["Stripe-Signature"];
            var webhookSecret = _config["Stripe:WebhookSecret"];

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    webhookSecret
                );
            }
            catch
            {
                return BadRequest();
            }

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;

                if (!intent.Metadata.ContainsKey("invoiceId"))
                    return Ok();

                var invoiceId = int.Parse(intent.Metadata["invoiceId"]);

                var invoice = await _db.Invoice
                    .Include(x => x.Payments)
                    .FirstOrDefaultAsync(x => x.InvoiceID == invoiceId);

                if (invoice != null && !invoice.Paid)
                {
                    invoice.Paid = true;

                    _db.Payment.Add(new Payments
                    {
                        InvoiceID = invoice.InvoiceID,
                        StudentID = invoice.StudentID,
                        Amount = invoice.TotalAmount,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = "Stripe",
                        PaymentStatus = "Succeeded",
                        ReferenceNumber = intent.Id
                    });

                    await _db.SaveChangesAsync();
                }
            }

            return Ok();
        }
    }

}
