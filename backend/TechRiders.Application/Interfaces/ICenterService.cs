using TechRiders.Application.DTOs.Requests.Center;
using TechRiders.Application.DTOs.Responses.Center;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de centros educativos
/// Define el contrato de operaciones de negocio para centros
/// </summary>
public interface ICenterService
{
    /// <summary>
    /// Obtiene todos los centros activos
    /// </summary>
    Task<IEnumerable<CenterResponse>> GetAllCentersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un centro por su ID
    /// </summary>
    Task<CenterResponse?> GetCenterByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca centros por término de búsqueda
    /// </summary>
    Task<IEnumerable<CenterResponse>> SearchCentersAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene centros por localidad
    /// </summary>
    Task<IEnumerable<CenterResponse>> GetCentersByLocalityAsync(string locality, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo centro
    /// </summary>
    Task<CenterResponse> CreateCenterAsync(CreateCenterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un centro existente
    /// </summary>
    Task<CenterResponse?> UpdateCenterAsync(Guid id, UpdateCenterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente un centro
    /// </summary>
    Task<bool> DeleteCenterAsync(Guid id, CancellationToken cancellationToken = default);
}
