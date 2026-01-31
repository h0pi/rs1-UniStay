using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace UniStay.API.Data.Models
{
    public class HallReservations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        public DateTime ReservedAt { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [ForeignKey("Hall")]
        public int HallID { get; set; }
        public Halls Hall { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Users Student { get; set; }
    }
}
