using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TechRidersDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<User>> GetActiveByCapabilityNameAsync(string capabilityName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.IsActive && u.Capabilities.Any(uc =>
                uc.Status == CapabilityStatus.Activa &&
                uc.Capability.Name.ToLower() == capabilityName.ToLower()))
            .OrderBy(u => u.Name)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }
}
