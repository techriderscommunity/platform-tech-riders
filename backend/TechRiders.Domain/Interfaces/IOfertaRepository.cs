using TechRiders.Domain.Entities.Empleo;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Ofertas de empleo
/// </summary>
public interface IOfertaRepository : IRepository<Oferta>
{
    /// <summary>
    /// Obtiene todas las ofertas activas
    /// </summary>
    Task<IEnumerable<Oferta>> GetActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ofertas por empresa
    /// </summary>
    Task<IEnumerable<Oferta>> GetByEmpresaAsync(string empresa, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ofertas por modalidad
    /// </summary>
    Task<IEnumerable<Oferta>> GetByModalidadAsync(Modalidad modalidad, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ofertas en un rango de fechas
    /// </summary>
    Task<IEnumerable<Oferta>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Búsqueda de ofertas por término
    /// </summary>
    Task<IEnumerable<Oferta>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
