using TechRiders.Application.DTOs.Requests.Ambassador;
using TechRiders.Application.DTOs.Responses.Ambassador;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de ambassadors
/// Define el contrato de operaciones de negocio para ambassadors
/// </summary>
public interface IAmbassadorService
{
    /// <summary>
    /// Obtiene todos los ambassadors activos
    /// </summary>
    Task<IEnumerable<AmbassadorResponse>> GetAllAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un ambassador por su ID
    /// </summary>
    Task<AmbassadorResponse?> GetAmbassadorByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca ambassadors por término de búsqueda
    /// </summary>
    Task<IEnumerable<AmbassadorResponse>> SearchAmbassadorsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors por categoría
    /// </summary>
    Task<IEnumerable<AmbassadorResponse>> GetAmbassadorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene ambassadors que están trabajando actualmente
    /// </summary>
    Task<IEnumerable<AmbassadorResponse>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo ambassador
    /// </summary>
    Task<AmbassadorResponse> CreateAmbassadorAsync(CreateAmbassadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un ambassador existente
    /// </summary>
    Task<AmbassadorResponse?> UpdateAmbassadorAsync(Guid id, UpdateAmbassadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente un ambassador
    /// </summary>
    Task<bool> DeleteAmbassadorAsync(Guid id, CancellationToken cancellationToken = default);
}
