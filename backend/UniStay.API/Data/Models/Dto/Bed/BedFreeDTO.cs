namespace UniStay.API.Data.Models.Dto.Bed
{
    public class BedFreeDTO
    {
        public int BedId { get; set; }
        public string BedNumber { get; set; }

        public int RoomId { get; set; }
        public string RoomNumber { get; set; }

        public int Floor { get; set; }
    }
}
