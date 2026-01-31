namespace UniStay.API.Data.Models.Dto.Invoice
{
    public class InvoiceGetDTO
    {
        public int InvoiceID { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public bool IssuedAt { get; set; }
    }
}
