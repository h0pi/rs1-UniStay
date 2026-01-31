using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class BedAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [ForeignKey("Bed")]
        public int BedID { get; set; }
        public Beds Bed { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Users Student { get; set; }
    }
}
