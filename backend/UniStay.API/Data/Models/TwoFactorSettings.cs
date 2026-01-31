// TwoFactorSettings.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniStay.API.Data.Models;

public class TwoFactorSettings
{
    [Key]
    public int UserID { get; set; } // PK = FK to Users

    public bool IsEnabled { get; set; }
    public DateTime? EnabledAt { get; set; }

    // optional: last method used (email/sms)
    public string Method { get; set; }

    public bool RequiresTwoFactor { get; set; }

}