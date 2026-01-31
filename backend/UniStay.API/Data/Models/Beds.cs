using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class Beds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BedID { get; set; }

        public string BedNumber { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public Rooms Room { get; set; }

        public ICollection<BedAssignments> BedAssignments { get; set; }

    }
}
