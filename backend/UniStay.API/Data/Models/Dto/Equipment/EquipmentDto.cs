namespace UniStay.API.Data.Models.Dto.Equipment
{
    public class EquipmentDto
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string? RentalPrice { get; set; }
        public string? EquipmentType { get; set; }
    }
}
