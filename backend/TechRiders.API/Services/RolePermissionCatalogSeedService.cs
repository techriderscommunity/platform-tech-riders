using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Catálogo de permisos funcionales de intranet y asignación heredada por rol.</summary>
public static class RolePermissionCatalogSeedService
{
    private static readonly string[] MemberPermissions =
    [
        "profile.manage", "preferences.manage", "favorites.manage", "events.register",
        "sessions.request", "communities.follow",
    ];

    private static readonly Dictionary<string, string[]> RolePermissions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Member"] = MemberPermissions,
        ["Ambassador"] = [.. MemberPermissions, "ambassador.profile.manage", "ambassador.availability.manage", "sessions.assigned.view", "sessions.assigned.respond", "sessions.history.view", "sessions.propose", "events.participate"],
        ["Community Partner"] = [.. MemberPermissions, "community.profile.manage", "community.info.manage", "events.create", "activities.create", "collaborations.propose"],
        ["Center"] = [.. MemberPermissions, "center.info.manage", "center.requests.view", "center.sessions.view", "center.sessions.request", "center.sessions.history.view", "center.contacts.manage"],
        ["Community Leader"] = [.. MemberPermissions, "community.events.approve", "community.activities.manage", "community.requests.validate", "community.initiatives.coordinate", "events.create", "community.manage"],
        ["Staff"] = [.. MemberPermissions, "role-requests.approve", "sessions.fptour.manage", "events.official.create", "community.manage", "taxonomy.manage", "content.manage", "validations.manage", "functional-config.manage", "privacy.manage"],
    };

    private static readonly string[] AdminPermissions =
    [
        "platform.manage", "security.manage", "audit.manage", "users.manage",
    ];

    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var allPermissionNames = RolePermissions.Values.SelectMany(x => x).Concat(AdminPermissions).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        var permissions = await dbContext.Set<Permission>().ToListAsync(cancellationToken);

        foreach (var permissionName in allPermissionNames)
        {
            if (permissions.Any(p => string.Equals(p.Name, permissionName, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = permissionName,
                Description = $"Permiso funcional de intranet: {permissionName}",
            };
            permissions.Add(permission);
            dbContext.Set<Permission>().Add(permission);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        foreach (var roleEntry in RolePermissions)
        {
            var role = await dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleEntry.Key, cancellationToken);
            if (role is null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = roleEntry.Key,
                    Description = "Rol de comunidad con permisos funcionales heredables.",
                };
                dbContext.Set<Role>().Add(role);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await EnsureRolePermissionsAsync(dbContext, role, roleEntry.Value, permissions, cancellationToken);
        }

        var admin = await dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);
        if (admin is not null)
        {
            var inheritedPermissions = RolePermissions.Values.SelectMany(x => x).Concat(AdminPermissions).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            await EnsureRolePermissionsAsync(dbContext, admin, inheritedPermissions, permissions, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Catálogo de permisos funcionales y herencia de roles verificado.");
    }

    private static async Task EnsureRolePermissionsAsync(TechRidersDbContext dbContext, Role role, IEnumerable<string> permissionNames, IReadOnlyCollection<Permission> permissions, CancellationToken cancellationToken)
    {
        var permissionIds = permissions
            .Where(p => permissionNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
            .Select(p => p.Id)
            .ToArray();
        var existingIds = await dbContext.Set<RolePermission>()
            .Where(rp => rp.RoleId == role.Id)
            .Select(rp => rp.PermissionId)
            .ToListAsync(cancellationToken);

        foreach (var permissionId in permissionIds.Where(id => !existingIds.Contains(id)))
        {
            dbContext.Set<RolePermission>().Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
        }
    }
}
