using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace UniStay.API.Data.Models
{

    public class EquipmentRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordID { get; set; }

        public DateTime? AssignedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentID { get; set; }        
        public Equipments Equipment { get; set; }

        [ForeignKey("Student")]
        public int? StudentID { get; set; }
        public Users Student { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }
        public Users Employee { get; set; }

        [Column("RecordSerialNumber")]
        public string SerialNumber { get; set; }
        public string? Location { get; set; }

        public bool IsAvailable { get; set; }
    }
}
