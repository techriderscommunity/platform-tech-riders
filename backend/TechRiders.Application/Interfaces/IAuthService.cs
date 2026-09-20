using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

public sealed record AuthUserProfile(
    string Id,
    string Email,
    string Name,
    string Role,
    IReadOnlyList<string> Roles,
    string? LinkedIn,
    string? Instagram,
    string? X,
    string? YouTube,
    string? Github);

public sealed record PasswordResetResult(bool Success, string Message, string Token);

/// <summary>Registro, login y recuperacion de contrasena contra usuarios reales de BD.</summary>
public interface IAuthService
{
    Task EnsureDefaultAdminAsync(CancellationToken cancellationToken = default);
    Task<User> RegisterAsync(string nickname, string name, string lastName, string email, string password, CancellationToken cancellationToken = default);
    Task<User?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<PasswordResetResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
    string CreateToken(User user);
    AuthUserProfile BuildUserProfile(User user);
}
