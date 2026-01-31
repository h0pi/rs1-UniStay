namespace UniStay.API.Data.Models.Dto.Room
{
    public class RoomDTO
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int MaxOccupancy { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<RoomUserDTO> Students { get; set; } = new ();
    }   
}
