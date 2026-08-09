using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Service interface for job offer and application management
/// </summary>
public interface IEmploymentService
{
    // Oferta operations
    
    /// <summary>
    /// Gets all active job offers
    /// </summary>
    Task<IEnumerable<OfertaResponse>> GetAllOfertasAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a job offer by ID
    /// </summary>
    Task<OfertaResponse?> GetOfertaByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new job offer
    /// </summary>
    Task<OfertaResponse> CreateOfertaAsync(CreateOfertaRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing job offer
    /// </summary>
    Task<OfertaResponse> UpdateOfertaAsync(UpdateOfertaRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a job offer (changes state from Draft to Active)
    /// </summary>
    Task<OfertaResponse> PublishOfertaAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes a job offer (changes state to Closed)
    /// </summary>
    Task<OfertaResponse> CloseOfertaAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a job offer (soft delete)
    /// </summary>
    Task DeleteOfertaAsync(Guid id, CancellationToken cancellationToken = default);

    // Candidatura operations

    /// <summary>
    /// Gets all applications for a specific job offer
    /// </summary>
    Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByOfertaAsync(Guid ofertaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all applications from a specific junior
    /// </summary>
    Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByJuniorAsync(string juniorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an application by ID
    /// </summary>
    Task<CandidaturaResponse?> GetCandidaturaByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new application for a job offer
    /// </summary>
    Task<CandidaturaResponse> CreateCandidaturaAsync(CreateCandidaturaRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves an application to interview stage
    /// </summary>
    Task<CandidaturaResponse> AdvanceToInterviewAsync(Guid candidaturaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects an application
    /// </summary>
    Task<CandidaturaResponse> RejectCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Hires a candidate (changes state to Hired)
    /// </summary>
    Task<CandidaturaResponse> HireCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an application (soft delete)
    /// </summary>
    Task DeleteCandidaturaAsync(Guid id, CancellationToken cancellationToken = default);
}
