using TechRiders.Application.DTOs.Requests.FPTour;
using TechRiders.Application.DTOs.Responses.FPTour;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de tours FP
/// Define el contrato de operaciones de negocio para tours
/// </summary>
public interface IFPTourService
{
    /// <summary>
    /// Obtiene todos los tours activos
    /// </summary>
    Task<IEnumerable<FPTourResponse>> GetAllFPToursAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un tour por su ID
    /// </summary>
    Task<FPTourResponse?> GetFPTourByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours por centro
    /// </summary>
    Task<IEnumerable<FPTourResponse>> GetFPToursByCenterAsync(Guid centerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours por ambassador
    /// </summary>
    Task<IEnumerable<FPTourResponse>> GetFPToursByAmbassadorAsync(Guid ambassadorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours pendientes (sin fecha agendada)
    /// </summary>
    Task<IEnumerable<FPTourResponse>> GetPendingFPToursAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo tour
    /// </summary>
    Task<FPTourResponse> CreateFPTourAsync(CreateFPTourRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un tour existente
    /// </summary>
    Task<FPTourResponse?> UpdateFPTourAsync(Guid id, UpdateFPTourRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente un tour
    /// </summary>
    Task<bool> DeleteFPTourAsync(Guid id, CancellationToken cancellationToken = default);
}
