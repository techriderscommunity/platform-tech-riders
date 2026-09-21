using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface IIntranetUserCategoryRepository : IRepository<IntranetUserCategory>
{
    Task<IEnumerable<IntranetUserCategory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetUserCategory>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetUserCategory>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> UserHasCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetUserCategory>> GetActiveAsync(CancellationToken cancellationToken = default);
}