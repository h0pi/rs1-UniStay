using Stripe;

namespace UniStay.API.Services
{
    public class StripeService
    {
        public PaymentIntent CreatePaymentIntent(decimal amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // KM → feninzi
                Currency = "bam",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            return service.Create(options);
        }
    }
}
