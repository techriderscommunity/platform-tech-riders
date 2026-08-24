using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Tutoriales
/// </summary>
public interface ITutorialRepository : IRepository<Tutorial>
{
    /// <summary>
    /// Obtiene un tutorial por su slug
    /// </summary>
    Task<Tutorial?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tutoriales por autor
    /// </summary>
    Task<IEnumerable<Tutorial>> GetByAutorAsync(string autor, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tutoriales por categoría
    /// </summary>
    Task<IEnumerable<Tutorial>> GetByCategoriaAsync(string categoria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tutoriales en un rango de fechas
    /// </summary>
    Task<IEnumerable<Tutorial>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Búsqueda de tutoriales
    /// </summary>
    Task<IEnumerable<Tutorial>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tutoriales paginados
    /// </summary>
    Task<TutorialesPageResult> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe un slug duplicado
    /// </summary>
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
