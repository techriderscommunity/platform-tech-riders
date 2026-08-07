using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities.Tutoriales;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de ITutorialRepository
/// Proporciona acceso a datos de Tutoriales
/// </summary>
public sealed class TutorialRepository : Repository<Tutorial>, ITutorialRepository
{
    public TutorialRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a tutorial by slug
    /// </summary>
    public async Task<Tutorial?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var tutorials = await FindAsync(
            predicate: t => t.IsActive && t.Slug == slug,
            cancellationToken: cancellationToken
        );
        return tutorials.FirstOrDefault();
    }

    /// <summary>
    /// Gets all tutorials by author
    /// </summary>
    public async Task<IEnumerable<Tutorial>> GetByAutorAsync(string autor, CancellationToken cancellationToken = default)
    {
        var tutoriales = await FindAsync(
            predicate: t => t.IsActive && t.Autor == autor,
            cancellationToken: cancellationToken
        );
        return tutoriales.OrderByDescending(t => t.FechaPublicacion);
    }

    /// <summary>
    /// Gets tutorials by category
    /// </summary>
    public async Task<IEnumerable<Tutorial>> GetByCategoriaAsync(string categoria, CancellationToken cancellationToken = default)
    {
        var tutoriales = await FindAsync(
            predicate: t => t.IsActive && t.CategoriasJson.Contains(categoria),
            cancellationToken: cancellationToken
        );
        return tutoriales.OrderByDescending(t => t.FechaPublicacion);
    }

    /// <summary>
    /// Gets tutorials within a date range
    /// </summary>
    public async Task<IEnumerable<Tutorial>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var tutoriales = await FindAsync(
            predicate: t => t.IsActive && t.FechaPublicacion >= startDate && t.FechaPublicacion <= endDate,
            cancellationToken: cancellationToken
        );
        return tutoriales.OrderByDescending(t => t.FechaPublicacion);
    }

    /// <summary>
    /// Searches tutorials by term
    /// </summary>
    public async Task<IEnumerable<Tutorial>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchQuery = searchTerm.ToLowerInvariant();
        var tutoriales = await FindAsync(
            predicate: t => t.IsActive && (
                t.Titulo.ToLower().Contains(searchQuery) ||
                t.Extracto.ToLower().Contains(searchQuery) ||
                t.Autor.ToLower().Contains(searchQuery)
            ),
            cancellationToken: cancellationToken
        );
        return tutoriales.OrderByDescending(t => t.FechaPublicacion);
    }

    /// <summary>
    /// Gets paginated tutorials
    /// </summary>
    public async Task<TutorialesPageResult> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        const int minPageSize = 1;
        const int maxPageSize = 100;

        var validPageSize = Math.Clamp(pageSize, minPageSize, maxPageSize);
        var skipCount = (pageNumber - 1) * validPageSize;

        var query = _dbSet.Where(t => t.IsActive).OrderByDescending(t => t.FechaPublicacion);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip(skipCount).Take(validPageSize).ToListAsync(cancellationToken);

        return new TutorialesPageResult(items.AsReadOnly(), total);
    }

    /// <summary>
    /// Checks if a slug already exists
    /// </summary>
    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(
            predicate: t => t.IsActive && t.Slug == slug && (!excludeId.HasValue || t.Id != excludeId.Value),
            cancellationToken: cancellationToken
        );
    }
}
