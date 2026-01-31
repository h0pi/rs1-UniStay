using Microsoft.AspNetCore.Identity;
using UniStay.API.Data.Models;

public static class SecurityHelpers
{
    // koristi Identity PasswordHasher za konzistentan hashing
    private static readonly PasswordHasher<Users> _hasher = new PasswordHasher<Users>();

    public static string HashAnswer(Users user, string plain)
    {
        return _hasher.HashPassword(user, plain);
    }

    public static PasswordVerificationResult VerifyAnswer(Users user, string hashed, string plain)
    {
        return _hasher.VerifyHashedPassword(user, hashed, plain);
    }

    public static string GenerateSecureToken(int length = 48)
    {
        var bytes = new byte[length];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        // base64-url safe
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}