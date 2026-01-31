using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models.Dto.Fault
{
    public class FaultCreateDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int RoomID { get; set; }


    }
}
