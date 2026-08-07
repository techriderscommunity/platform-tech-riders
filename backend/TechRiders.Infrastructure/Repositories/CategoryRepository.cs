using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TechRidersDbContext _context;
    private readonly DbSet<MT_Category> _dbSet;

    public CategoryRepository(TechRidersDbContext context)
    {
        _context = context;
        _dbSet = context.Set<MT_Category>();
    }

    public async Task<MT_Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<MT_Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MT_Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Active)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MT_Category>> GetMainCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Secondary)
            .Where(c => c.Active && c.FatherId == null)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MT_Category>> GetSubCategoriesAsync(int fatherId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Active && c.FatherId == fatherId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<MT_Category?> GetCategoryWithSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Main)
            .Include(c => c.Secondary.Where(s => s.Active))
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }

    public async Task<MT_Category> AddAsync(MT_Category category, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(category, cancellationToken);
        return category;
    }

    public Task UpdateAsync(MT_Category category, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(category);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(MT_Category category, CancellationToken cancellationToken = default)
    {
        category.Active = false;
        _dbSet.Update(category);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.Active, cancellationToken);
    }
}
