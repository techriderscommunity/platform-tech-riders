using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Tours FP
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface IFPTourRepository : IRepository<FPTour>
{
    /// <summary>
    /// Obtiene tours activos
    /// </summary>
    Task<IEnumerable<FPTour>> GetActiveFPToursAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un tour con centro y ambassador incluidos
    /// </summary>
    Task<FPTour?> GetFPTourWithDetailsAsync(Guid tourId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours por centro
    /// </summary>
    Task<IEnumerable<FPTour>> GetFPToursByCenterAsync(Guid centerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours por ambassador
    /// </summary>
    Task<IEnumerable<FPTour>> GetFPToursByAmbassadorAsync(Guid ambassadorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene tours pendientes (sin fecha agendada)
    /// </summary>
    Task<IEnumerable<FPTour>> GetPendingFPToursAsync(CancellationToken cancellationToken = default);
}
