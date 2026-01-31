using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models.Dto.User
{
    public class UserUpdateDTO
    {
        [Required]
        public int UserID { get; set; }   

        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? ProfileImage { get; set; }

        public int? RoleID { get; set; }
    }
}

