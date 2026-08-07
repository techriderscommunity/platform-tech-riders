using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Centros
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface ICenterRepository : IRepository<Center>
{
    /// <summary>
    /// Obtiene centros activos
    /// </summary>
    Task<IEnumerable<Center>> GetActiveCentersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca centros por nombre o email
    /// </summary>
    Task<IEnumerable<Center>> SearchCentersAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene centros por localidad
    /// </summary>
    Task<IEnumerable<Center>> GetCentersByLocalityAsync(string locality, CancellationToken cancellationToken = default);
}
