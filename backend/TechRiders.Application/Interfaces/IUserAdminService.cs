using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

public sealed record UserActivitySummary(int EventsRegistered, int SessionsRegistered, int SpeakerSessions, int FPToursAsAmbassador, List<IntranetAuditLog> RecentActions);

/// <summary>Gestion completa de usuarios (alta, edicion, activacion/baja, roles) para el panel de Staff/Admin.</summary>
public interface IUserAdminService
{
    Task<(List<User> Items, int TotalCount)> ListAsync(string? search, string? role, string? membershipStatus, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(string nickname, string name, string lastName, string email, string? phone, string? locality, string? about, CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(Guid id, string name, string lastName, string email, string? phone, string? locality, string? about, CancellationToken cancellationToken = default);
    Task<User> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task RevokeRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<UserActivitySummary> GetActivityAsync(Guid userId, CancellationToken cancellationToken = default);
}
