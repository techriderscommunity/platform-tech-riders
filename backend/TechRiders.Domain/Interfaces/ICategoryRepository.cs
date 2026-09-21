using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<MT_Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<MT_Category>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<MT_Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<MT_Category>> GetMainCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<MT_Category>> GetSubCategoriesAsync(int fatherId, CancellationToken cancellationToken = default);

    Task<MT_Category?> GetCategoryWithSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<MT_Category> AddAsync(MT_Category category, CancellationToken cancellationToken = default);

    Task UpdateAsync(MT_Category category, CancellationToken cancellationToken = default);

    Task DeleteAsync(MT_Category category, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}