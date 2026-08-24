using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Candidaturas
/// </summary>
public interface ICandidaturaRepository : IRepository<Candidatura>
{
    /// <summary>
    /// Obtiene candidaturas de una oferta específica
    /// </summary>
    Task<IEnumerable<Candidatura>> GetByOfertaIdAsync(Guid ofertaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene candidaturas de un junior
    /// </summary>
    Task<IEnumerable<Candidatura>> GetByJuniorIdAsync(string juniorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene candidaturas por estado
    /// </summary>
    Task<IEnumerable<Candidatura>> GetByEstadoAsync(CandidaturaEstado estado, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene candidaturas contratadas para una empresa
    /// </summary>
    Task<IEnumerable<Candidatura>> GetContratadasAsync(Guid ofertaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si ya existe una candidatura
    /// </summary>
    Task<bool> ExisteCandidaturaAsync(Guid ofertaId, string juniorId, CancellationToken cancellationToken = default);
}
