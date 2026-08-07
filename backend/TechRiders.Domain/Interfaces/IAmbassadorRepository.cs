using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Ambassadors
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface IAmbassadorRepository : IRepository<Ambassador>
{
    /// <summary>
    /// Obtiene ambassadors activos
    /// </summary>
    Task<IEnumerable<Ambassador>> GetActiveAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca ambassadors por nombre, apellido o email
    /// </summary>
    Task<IEnumerable<Ambassador>> SearchAmbassadorsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors por categoría
    /// </summary>
    Task<IEnumerable<Ambassador>> GetAmbassadorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors que están trabajando actualmente
    /// </summary>
    Task<IEnumerable<Ambassador>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un ambassador con su categoría incluida
    /// </summary>
    Task<Ambassador?> GetAmbassadorWithCategoryAsync(Guid ambassadorId, CancellationToken cancellationToken = default);
}
