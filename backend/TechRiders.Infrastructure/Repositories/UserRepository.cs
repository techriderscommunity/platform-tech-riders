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
        var normalizedCapabilityName = capabilityName.Trim();
        var requestedName = normalizedCapabilityName.ToLower();
        var isStaffLookup = requestedName == "staff" || requestedName == "admin";

        return await _dbSet
            .Where(u => u.IsActive && (
                u.Capabilities.Any(uc =>
                    uc.Status == CapabilityStatus.Activa &&
                    (
                        uc.Capability.Name.ToLower() == requestedName ||
                        (isStaffLookup && (
                            uc.Capability.Name.ToLower() == "staff" ||
                            uc.Capability.Name.ToLower() == "admin"))
                    ))
                ||
                u.UserRoles.Any(ur =>
                    ur.Role.Name.ToLower() == requestedName ||
                    (isStaffLookup && (
                        ur.Role.Name.ToLower() == "staff" ||
                        ur.Role.Name.ToLower() == "admin"))
                ))
            )
            .OrderBy(u => u.Name)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetActiveMembersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.IsActive
                && !u.Capabilities.Any(uc =>
                    uc.Status == CapabilityStatus.Activa && (
                        uc.Capability.Name.ToLower() == "staff" ||
                        uc.Capability.Name.ToLower() == "admin" ||
                        uc.Capability.Name.ToLower() == "community leader" ||
                        uc.Capability.Name.ToLower() == "ambassador"))
                && !u.UserRoles.Any(ur =>
                    ur.Role.Name.ToLower() == "staff" ||
                    ur.Role.Name.ToLower() == "admin" ||
                    ur.Role.Name.ToLower() == "community leader" ||
                    ur.Role.Name.ToLower() == "ambassador"))
            .OrderBy(u => u.Name)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }
}
