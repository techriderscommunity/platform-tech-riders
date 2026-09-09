using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public sealed class CommunityPartnerApplicationRepository : Repository<CommunityPartnerApplication>, ICommunityPartnerApplicationRepository
{
    public CommunityPartnerApplicationRepository(TechRidersDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsDuplicateAsync(
        string name,
        string website,
        string contactEmail,
        CancellationToken cancellationToken = default)
    {
        return _dbSet.AnyAsync(application =>
            application.IsActive &&
            (application.Name == name || application.Website == website || application.ContactEmail == contactEmail),
            cancellationToken);
    }
}