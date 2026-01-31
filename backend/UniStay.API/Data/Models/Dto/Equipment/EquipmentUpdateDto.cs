namespace UniStay.API.Data.Models.Dto.Equipment
{
    public class EquipmentUpdateDto
    {
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string? RentalPrice { get; set; }
    }
}