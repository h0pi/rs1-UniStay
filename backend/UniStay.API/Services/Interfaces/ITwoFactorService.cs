public interface ITwoFactorService
{
    Task SendTwoFactorCodeAsync(int userId, string email, CancellationToken ct = default);
    Task<bool> VerifyTwoFactorCodeAsync(int userId, string code, CancellationToken ct = default);
    Task<IEnumerable<string>> GenerateBackupCodesAsync(int userId, int count = 8, CancellationToken ct = default);
    Task<bool> VerifyBackupCodeAsync(int userId, string code, CancellationToken ct = default);
    Task DisableTwoFactorAsync(int userId, CancellationToken ct = default);
    Task<string> GenerateAndSendCode(int userId, CancellationToken ct = default);
}