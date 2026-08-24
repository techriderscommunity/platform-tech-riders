using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class CenterRepository : Repository<Center>, ICenterRepository
{
    public CenterRepository(TechRidersDbContext context) : base(context) { }

    public async Task<IEnumerable<Center>> GetActiveCentersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Center>> SearchCentersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetActiveCentersAsync(cancellationToken);
        }

        var normalizedSearch = searchTerm.Trim();

        return await _dbSet
            .Where(c => c.IsActive &&
                        ((c.Name != null && c.Name.Contains(normalizedSearch)) ||
                         (c.Email != null && c.Email.Contains(normalizedSearch))))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Center>> GetCentersByLocalityAsync(string locality, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(locality))
        {
            return await GetActiveCentersAsync(cancellationToken);
        }

        var normalizedLocality = locality.Trim();

        return await _dbSet
            .Where(c => c.IsActive && c.Locality != null && c.Locality.Contains(normalizedLocality))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}
