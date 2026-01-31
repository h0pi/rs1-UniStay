namespace UniStay.API.Data.Models.Dto.Fault
{
    public class FaultFilterDto
    {
        public string? Title { get; set; }
        public int? ReportedBy { get; set; }
        public bool? IsResolved { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
