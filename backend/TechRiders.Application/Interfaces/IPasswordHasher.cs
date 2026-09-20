namespace TechRiders.Application.Interfaces;

/// <summary>Hashing/verificacion de contrasenas (PBKDF2). Aislado de IAuthService para poder testearlo y sustituirlo de forma independiente.</summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string storedHash);
}
