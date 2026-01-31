namespace UniStay.API.Data.Models.Dto.BedAssignment
{
    public class BedAssignmentRoomDTO
    {
        public int AssignmentID { get; set; }
        public int BedID { get; set; }
        public string BedNumber { get; set; }

        public int StudentID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }

}
