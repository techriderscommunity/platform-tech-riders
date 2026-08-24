using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Ambassadors
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface IAmbassadorRepository : IRepository<User>
{
    /// <summary>
    /// Obtiene ambassadors activos
    /// </summary>
    Task<IEnumerable<User>> GetActiveAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca ambassadors por nombre, apellido o email
    /// </summary>
    Task<IEnumerable<User>> SearchAmbassadorsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors por categoría
    /// </summary>
    Task<IEnumerable<User>> GetAmbassadorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors que están trabajando actualmente
    /// </summary>
    Task<IEnumerable<User>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un ambassador con su categoría incluida
    /// </summary>
    Task<User?> GetAmbassadorWithDetailsAsync(Guid ambassadorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si un usuario está marcado como ambassador activo
    /// </summary>
    Task<bool> IsAmbassadorAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Garantiza que el usuario tenga el rol ambassador
    /// </summary>
    Task EnsureAmbassadorRoleAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina el rol ambassador del usuario si existe
    /// </summary>
    Task RemoveAmbassadorRoleAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el número de ambassadors activos
    /// </summary>
    Task<int> CountActiveAmbassadorsAsync(CancellationToken cancellationToken = default);
}
