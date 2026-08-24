using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de IIntranetUserCategoryRepository
/// Proporciona acceso a datos de Categorías de Usuario de Intranet
/// </summary>
public sealed class IntranetUserCategoryRepository : Repository<IntranetUserCategory>, IIntranetUserCategoryRepository
{
    public IntranetUserCategoryRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets all categories for a user
    /// </summary>
    public async Task<IEnumerable<IntranetUserCategory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var categories = await FindAsync(
            predicate: uc => uc.IsActive && uc.UserId == userId,
            cancellationToken: cancellationToken
        );
        return categories.OrderBy(uc => uc.Category);
    }

    /// <summary>
    /// Gets all users in a category
    /// </summary>
    public async Task<IEnumerable<IntranetUserCategory>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var userCategories = await FindAsync(
            predicate: uc => uc.IsActive && uc.Category == category,
            cancellationToken: cancellationToken
        );
        return userCategories.OrderBy(uc => uc.UserId);
    }

    /// <summary>
    /// Gets active categories for a user
    /// </summary>
    public async Task<IEnumerable<IntranetUserCategory>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var categories = await FindAsync(
            predicate: uc => uc.IsActive && uc.UserId == userId && uc.Active,
            cancellationToken: cancellationToken
        );
        return categories.OrderBy(uc => uc.Category);
    }

    /// <summary>
    /// Checks if a user has a specific category
    /// </summary>
    public async Task<bool> UserHasCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(
            predicate: uc => uc.IsActive && uc.UserId == userId && uc.Category == category,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Gets all active user categories
    /// </summary>
    public async Task<IEnumerable<IntranetUserCategory>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var categories = await FindAsync(
            predicate: uc => uc.IsActive && uc.Active,
            cancellationToken: cancellationToken
        );
        return categories.OrderBy(uc => uc.UserId).ThenBy(uc => uc.Category);
    }
}
