using System.Security.Cryptography;
using TechRiders.Application.Interfaces;

namespace TechRiders.Infrastructure.Services;

/// <summary>Hashing PBKDF2 (200k iteraciones, SHA-256), extraido de DatabaseAuthService sin cambio de algoritmo.</summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int PBKDF2Iterations = 200_000;
    private const int KeyBytes = 32;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PBKDF2Iterations, HashAlgorithmName.SHA256, KeyBytes);
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        var separatorIndex = storedHash.IndexOf(':');
        if (separatorIndex <= 0 || separatorIndex == storedHash.Length - 1)
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(storedHash[..separatorIndex]);
            var expectedHash = Convert.FromBase64String(storedHash[(separatorIndex + 1)..]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PBKDF2Iterations, HashAlgorithmName.SHA256, expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
