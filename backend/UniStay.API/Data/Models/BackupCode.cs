// BackupCode.cs
using System.ComponentModel.DataAnnotations;

public class BackupCode
{
    [Key]
    public int Id { get; set; }
    public int UserID { get; set; }
    public string CodeHash { get; set; } = string.Empty;
    public bool Used { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}