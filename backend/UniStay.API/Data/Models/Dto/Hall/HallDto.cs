namespace UniStay.API.Dto.Hall
{
    public class HallDto
    {
        public int HallID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
    }
}