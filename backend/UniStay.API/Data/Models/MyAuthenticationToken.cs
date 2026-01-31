using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models
{
    public class MyAuthenticationToken
    {

            [Key]
            public int TokenID { get; set; }

            [Required]
            public string Value { get; set; } = string.Empty; 

            public string IpAddress { get; set; } = string.Empty;
            public DateTime RecordedAt { get; set; } = DateTime.Now;

            [ForeignKey(nameof(User))]
            public int UserID { get; set; }
            public Users? User { get; set; }
        }
    }

