using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace UniStay.API.Data.Models.Dto.Fault
{
    public class FaultListDto
    {
        public int FaultID { get; set; }
        public string Title { get; set; }
        public int ReportedByUserID {get;set;}
        public string ReportedByUserName { get; set; }
        public string Description { get; set; }
        public bool IsResolved { get; set; }
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Priority { get; set; }
        public int RoomID { get; set; }
    }
}
