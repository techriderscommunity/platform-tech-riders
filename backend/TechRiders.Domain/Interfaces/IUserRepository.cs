using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>Repositorio de Usuarios.</summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>Usuarios activos con la capability o el rol de comunidad indicado.</summary>
    Task<IReadOnlyList<User>> GetActiveByCapabilityNameAsync(string capabilityName, CancellationToken cancellationToken = default);

    /// <summary>Usuarios activos que no pertenecen a Staff, Community Leader o Ambassador.</summary>
    Task<IReadOnlyList<User>> GetActiveMembersAsync(CancellationToken cancellationToken = default);
}
