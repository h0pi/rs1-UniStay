using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Dto.Hall
{
    public class HallCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Range(1, 1000, ErrorMessage = "Kapacitet mora biti između 1 i 1000.")]
        public int Capacity { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}