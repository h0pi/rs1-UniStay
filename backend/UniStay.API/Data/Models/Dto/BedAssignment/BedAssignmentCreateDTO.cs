namespace UniStay.API.Data.Models.Dto.BedAssignment
{
    public class BedAssignmentCreateDTO
    {
        public int BedID { get; set; }
        public int StudentID { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
