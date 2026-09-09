using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface ICommunityPartnerApplicationRepository : IRepository<CommunityPartnerApplication>
{
    Task<bool> ExistsDuplicateAsync(
        string name,
        string website,
        string contactEmail,
        CancellationToken cancellationToken = default);
}