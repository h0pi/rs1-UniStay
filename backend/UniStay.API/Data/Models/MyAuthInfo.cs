using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;

namespace UniStay.API.Data.Models
{
    public class MyAuthInfo
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        [Column("Role")]
        public string? RoleName { get; set; }
        public int? RoleId { get; set; }
        public bool IsLoggedIn { get; set; }


        public class AuthService
        {
            private readonly ApplicationDbContext _context;
            public AuthService(ApplicationDbContext context)
            {
                _context = context;
            }
        
        public async Task<bool> SetUserLoggedInAsync(int userId)
        {

                var authInfo = await _context.MyAuthInfo.FirstOrDefaultAsync(a => a.UserId == userId);
                if (authInfo != null)
                {
                    authInfo.IsLoggedIn = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            
        }
    } }
}