using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>Repositorio de Usuarios.</summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>Usuarios activos con una capability aprobada (Activa) por nombre de capability.</summary>
    Task<IReadOnlyList<User>> GetActiveByCapabilityNameAsync(string capabilityName, CancellationToken cancellationToken = default);
}
