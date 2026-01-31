using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniStay.API.Data.Models.Dto.Fault
{
    public class FaultUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? Priority { get; set; }
        public bool? IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }

    }
}
