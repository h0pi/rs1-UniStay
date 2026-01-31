using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class Faults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FaultID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? Priority { get; set; }
        public bool? IsResolved { get; set; }

        [ForeignKey("ReportedByUser")]
        public int ReportedByUserID { get; set; }
        public Users ReportedByUser { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public Rooms Room { get; set; }
    }
}
