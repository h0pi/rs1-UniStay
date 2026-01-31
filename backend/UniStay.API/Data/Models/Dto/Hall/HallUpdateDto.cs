using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Dto.Hall
{
    public class HallUpdateDto : HallCreateDto
    {
        [Required]
        public int HallID { get; set; }
    }
}