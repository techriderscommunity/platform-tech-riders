using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class AmbassadorRepository : Repository<Ambassador>, IAmbassadorRepository
{
    public AmbassadorRepository(TechRidersDbContext context) : base(context) { }

    public async Task<IEnumerable<Ambassador>> GetActiveAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Where(a => a.IsActive)
            .OrderBy(a => a.LastName).ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ambassador>> SearchAmbassadorsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Where(a => a.IsActive &&
                        (a.Name.Contains(searchTerm) ||
                         a.LastName.Contains(searchTerm) ||
                         a.Email.Contains(searchTerm)))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ambassador>> GetAmbassadorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Where(a => a.IsActive && a.CategoryId == categoryId)
            .OrderBy(a => a.LastName).ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ambassador>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Where(a => a.IsActive && a.IsWorking)
            .OrderBy(a => a.LastName).ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ambassador?> GetAmbassadorWithCategoryAsync(Guid ambassadorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Id == ambassadorId, cancellationToken);
    }
}
