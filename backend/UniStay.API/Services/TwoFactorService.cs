using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;

public class TwoFactorService : ITwoFactorService
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;
    private readonly int _codeLength = 6;
    private readonly TimeSpan _codeTtl = TimeSpan.FromMinutes(10);

    public TwoFactorService(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // --------------------------------------------------------
    // HELPERS
    // --------------------------------------------------------

    private string ComputeHash(string value)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
        return Convert.ToBase64String(bytes);
    }

    private string GenerateNumericCode(int digits)
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var val = Math.Abs(BitConverter.ToInt32(bytes, 0));
        var max = (int)Math.Pow(10, digits);
        return (val % max).ToString().PadLeft(digits, '0');
    }

    // --------------------------------------------------------
    // 2FA SEND CODE (LOGIN)
    // --------------------------------------------------------

    public async Task<string> GenerateAndSendCode(int userId, CancellationToken ct = default)
    {
        var user = await _db.User.FindAsync(userId);
        if (user == null)
            return null;

        var code = GenerateNumericCode(_codeLength);
        var hash = ComputeHash(code);

        var entry = new TwoFactorCode
        {
            UserID = userId,
            CodeHash = hash,
            ExpiresAt = DateTime.UtcNow.Add(_codeTtl),
            Used = false
        };

        _db.TwoFactorCode.Add(entry);
        await _db.SaveChangesAsync(ct);

        await _emailService.SendEmailAsync(
            user.Email,
            "Your UniStay 2FA Code",
            $"Your verification code is: <b>{code}</b>. It expires in {_codeTtl.TotalMinutes} minutes."
        );

        return code;
    }

    // --------------------------------------------------------
    // Legacy send-code endpoint support
    // --------------------------------------------------------

    public async Task SendTwoFactorCodeAsync(int userId, string email, CancellationToken ct = default)
    {
        await GenerateAndSendCode(userId, ct);
    }

    // --------------------------------------------------------
    // VERIFY LOGIN CODE
    // --------------------------------------------------------

    public async Task<bool> VerifyTwoFactorCodeAsync(int userId, string code, CancellationToken ct = default)
    {
        var hashed = ComputeHash(code);

        var codes = await _db.TwoFactorCode
            .Where(x =>
                x.UserID == userId &&
                !x.Used &&
                x.ExpiresAt >= DateTime.UtcNow)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

        var match = codes.FirstOrDefault(x => x.CodeHash == hashed);

        if (match == null)
            return false;

        match.Used = true;
        await _db.SaveChangesAsync(ct);

        return true;
    }

    // --------------------------------------------------------
    // BACKUP CODES
    // --------------------------------------------------------

    public async Task<IEnumerable<string>> GenerateBackupCodesAsync(int userId, int count = 8, CancellationToken ct = default)
    {
        var old = _db.BackupCode.Where(x => x.UserID == userId);
        _db.BackupCode.RemoveRange(old);

        var plainList = new List<string>();

        for (int i = 0; i < count; i++)
        {
            var code = Guid.NewGuid().ToString("n").Substring(0, 8).ToUpper();
            plainList.Add(code);

            _db.BackupCode.Add(new BackupCode
            {
                UserID = userId,
                CodeHash = ComputeHash(code),
                Used = false
            });
        }

        await _db.SaveChangesAsync(ct);
        return plainList;
    }

    public async Task<bool> VerifyBackupCodeAsync(int userId, string code, CancellationToken ct = default)
    {
        var hashed = ComputeHash(code);

        var entry = await _db.BackupCode
            .FirstOrDefaultAsync(x =>
                x.UserID == userId &&
                !x.Used &&
                x.CodeHash == hashed, ct);

        if (entry == null)
            return false;

        entry.Used = true;
        await _db.SaveChangesAsync(ct);

        return true;
    }

    // --------------------------------------------------------
    // DISABLE 2FA
    // --------------------------------------------------------

    public async Task DisableTwoFactorAsync(int userId, CancellationToken ct = default)
    {
        var settings = await _db.TwoFactorSettings.FindAsync(userId);

        if (settings != null)
        {
            settings.RequiresTwoFactor = false;  // FIXED
            settings.EnabledAt = null;
            await _db.SaveChangesAsync(ct);
        }
    }
}