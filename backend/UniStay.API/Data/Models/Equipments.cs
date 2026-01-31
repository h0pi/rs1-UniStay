using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models
{
    public class Equipments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipmentID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string RentalPrice { get; set; }
        public string EquipmentType { get; set; }

        public ICollection<EquipmentRecords> EquipmentRecords { get; set; }

    }
}
