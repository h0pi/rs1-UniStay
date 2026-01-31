using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string ReferenceNumber { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Users Student { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceID { get; set; }
        public Invoices Invoice { get; set; }
    }
}
