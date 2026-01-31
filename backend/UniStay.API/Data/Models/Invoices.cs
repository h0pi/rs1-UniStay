using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class Invoices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceID { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IssuedAt { get; set; }
        public bool Paid { get; set; }
        public bool EmailSent { get; set; }

        [ForeignKey("Student")]        
        public int StudentID { get; set; }
        public Users Student { get; set; }

        public ICollection<Payments> Payments { get; set; }
    }
}
