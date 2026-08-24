using TechRiders.Application.DTOs.Requests.Knowledge;
using TechRiders.Application.DTOs.Responses.Knowledge;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Service interface for tutorial management
/// </summary>
public interface ITutorialsService
{
    /// <summary>
    /// Gets all active tutorials
    /// </summary>
    Task<IEnumerable<TutorialResponse>> GetAllTutorialsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a tutorial by ID
    /// </summary>
    Task<TutorialResponse?> GetTutorialByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a tutorial by slug
    /// </summary>
    Task<TutorialResponse?> GetTutorialBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets tutorials by author
    /// </summary>
    Task<IEnumerable<TutorialResponse>> GetTutorialsByAuthorAsync(string author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets tutorials by category
    /// </summary>
    Task<IEnumerable<TutorialResponse>> GetTutorialsByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated tutorial results
    /// </summary>
    Task<(IEnumerable<TutorialResponse> Items, int TotalCount)> GetPaginatedTutorialsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new tutorial
    /// </summary>
    Task<TutorialResponse> CreateTutorialAsync(CreateTutorialRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing tutorial
    /// </summary>
    Task<TutorialResponse> UpdateTutorialAsync(UpdateTutorialRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a tutorial (soft delete)
    /// </summary>
    Task DeleteTutorialAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a slug is already in use
    /// </summary>
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
