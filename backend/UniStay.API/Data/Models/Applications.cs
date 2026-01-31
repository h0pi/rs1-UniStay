using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models
{
    public class Applications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationID { get; set; }

        public DateTime AppliedAt { get; set; }
        public DateTime DecisionAt { get; set; }
        public string PreferredRoomType { get; set; }
        public string Notes { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Users Student { get; set; }

        [ForeignKey("AssignedRoom")]
        public int AssignedRoomID { get; set; }
        public Rooms AssignedRoom { get; set; }

        [ForeignKey("DecisionByUser")]
        public int? DecisionByUserID { get; set; }
        public Users? DecisionByUser { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
