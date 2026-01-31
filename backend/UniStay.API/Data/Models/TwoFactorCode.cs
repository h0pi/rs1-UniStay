// TwoFactorCode.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TwoFactorCode
{
    [Key]
    public int Id { get; set; }
    public int UserID { get; set; }
    public string CodeHash { get; set; } = string.Empty; // hashed code
    public DateTime ExpiresAt { get; set; }
    public bool Used { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}