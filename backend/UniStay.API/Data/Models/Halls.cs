using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models
{
    public class Halls
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HallID { get; set; }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Rooms> Rooms { get; set; }
        public ICollection<HallReservations> HallReservations { get; set; }

    }
}
