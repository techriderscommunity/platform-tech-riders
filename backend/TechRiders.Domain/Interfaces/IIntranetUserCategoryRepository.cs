using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Categorías de Usuario de Intranet
/// </summary>
public interface IIntranetUserCategoryRepository : IRepository<IntranetUserCategory>
{
    /// <summary>
    /// Obtiene categorías de un usuario
    /// </summary>
    Task<IEnumerable<IntranetUserCategory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene usuarios de una categoría específica
    /// </summary>
    Task<IEnumerable<IntranetUserCategory>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene categorías activas de un usuario
    /// </summary>
    Task<IEnumerable<IntranetUserCategory>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si un usuario tiene una categoría específica
    /// </summary>
    Task<bool> UserHasCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene categorías activas
    /// </summary>
    Task<IEnumerable<IntranetUserCategory>> GetActiveAsync(CancellationToken cancellationToken = default);
}
