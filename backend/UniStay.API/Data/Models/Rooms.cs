using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models
{
    public class Rooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomID { get; set; }

        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public int MaxOccupancy { get; set; }
        public string Description { get; set; }

        public ICollection<Beds> Beds { get; set; }
        public ICollection<Faults> Faults { get; set; }
        public ICollection<Applications> Applications { get; set; }
    }
}
