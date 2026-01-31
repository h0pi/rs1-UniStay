using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models.Dto.Room
{
    public class RoomUpdateDTO
    {
        [Required]
        public int RoomID { get; set; }
        public string? RoomNumber { get; set; }
        public int? Floor { get; set; }
        public int? MaxOccupancy { get; set; }
        public string? Description { get; set; }
    }
}
