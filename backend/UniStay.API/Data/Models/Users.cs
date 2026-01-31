using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;


namespace UniStay.API.Data.Models
{
    public class Users
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("Role")]
        public int? RoleID { get; set; }
        public Roles? Role { get; set; }

        public ICollection<Applications> Applications { get; set; }
        public ICollection<Messages> SentMessages { get; set; }
        public ICollection<Messages> ReceivedMessages { get; set; }
        public ICollection<Announcements> Announcements { get; set; }
        public ICollection<EquipmentRecords> EquipmentRecords { get; set; }
        public ICollection<Faults> Faults { get; set; }
        public ICollection<Invoices> Invoices { get; set; }
        public ICollection<HallReservations> HallReservations { get; set; }
        public ICollection<MyAuthenticationToken>? Tokens { get; set; }


        [NotMapped]
        public bool IsAdmin => Role != null && Role.RoleName == "Admin";

        [NotMapped]
        public bool IsStudent => Role != null && Role.RoleName == "Student";

        [NotMapped]
        public bool IsEmployee => Role != null && Role.RoleName == "Employee";


        public bool VerifyPassword(string password)
        {
            
            using (var sha = SHA256.Create())
            {
                var computedHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return computedHash == this.PasswordHash;
            }
        }

    }
}

