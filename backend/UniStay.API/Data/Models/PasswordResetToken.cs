// PasswordResetToken.cs
using UniStay.API.Data.Models;

public class PasswordResetToken
{
    public int PasswordResetTokenID { get; set; }
    public int UserID { get; set; }
    public Users User { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool Used { get; set; }
    public DateTime CreatedAt { get; set; }
}