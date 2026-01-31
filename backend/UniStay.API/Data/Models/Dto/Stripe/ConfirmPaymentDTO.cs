namespace UniStay.API.Data.Models.Dto.Stripe
{
    public class ConfirmPaymentDTO
    {
        public int InvoiceId { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
