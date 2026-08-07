using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class FPTourRepository : Repository<FPTour>, IFPTourRepository
{
    public FPTourRepository(TechRidersDbContext context) : base(context) { }

    public async Task<IEnumerable<FPTour>> GetActiveFPToursAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Center)
            .Include(t => t.Ambassador)
            .Where(t => t.IsActive)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<FPTour?> GetFPTourWithDetailsAsync(Guid tourId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Center)
            .Include(t => t.Ambassador)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);
    }

    public async Task<IEnumerable<FPTour>> GetFPToursByCenterAsync(Guid centerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Center)
            .Include(t => t.Ambassador)
            .Where(t => t.IsActive && t.CenterId == centerId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FPTour>> GetFPToursByAmbassadorAsync(Guid ambassadorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Center)
            .Include(t => t.Ambassador)
            .Where(t => t.IsActive && t.AmbassadorId == ambassadorId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FPTour>> GetPendingFPToursAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Center)
            .Include(t => t.Ambassador)
            .Where(t => t.IsActive && !t.HasScheduledDate)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
