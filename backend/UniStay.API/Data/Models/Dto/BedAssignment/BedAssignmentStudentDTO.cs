namespace UniStay.API.Data.Models.Dto.BedAssignment
{
    public class BedAssignmentStudentDTO
    {

        public int AssignmentID { get; set; }
        public int BedID { get; set; }
        public string BedNumber { get; set; }

        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
    }

}
