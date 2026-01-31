namespace UniStay.API.Data.Models.Dto.Stripe
{
    public class PaymentIntentResponseDTO
    {
        public string ClientSecret { get; set; }
        public decimal Amount { get; set; }
    }
}
