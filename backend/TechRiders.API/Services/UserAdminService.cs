using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Gestión completa de usuarios (alta, edición, activación/baja, roles) para el panel de Staff/Admin.</summary>
public static class UserAdminService
{
    public static async Task<(List<User> Items, int TotalCount)> ListAsync(
        TechRidersDbContext dbContext, string? search, string? role, string? membershipStatus,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .Include(u => u.Membership)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(u => u.Name.Contains(term) || u.LastName.Contains(term) || u.Email.Contains(term) || u.Nickname.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name == role));
        }

        if (!string.IsNullOrWhiteSpace(membershipStatus) && Enum.TryParse<MembershipStatus>(membershipStatus, ignoreCase: true, out var parsedStatus))
        {
            query = query.Where(u => u.Membership != null && u.Membership.Status == parsedStatus);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(u => u.Name).ThenBy(u => u.LastName)
            .Skip(Math.Max(page - 1, 0) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public static Task<User?> GetByIdAsync(TechRidersDbContext dbContext, Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Users
            .Include(u => u.Membership)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Capabilities).ThenInclude(c => c.Capability)
            .Include(u => u.ProfileHistories).ThenInclude(ph => ph.Profile)
            .Include(u => u.OrganizationRelations).ThenInclude(po => po.Organization)
            .Include(u => u.UserSkills).ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public static async Task<User> CreateAsync(
        TechRidersDbContext dbContext, string nickname, string name, string lastName, string email,
        string? phone, string? locality, string? about, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        if (await dbContext.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe una cuenta registrada con ese correo.");
        }

        var memberRole = await GetOrCreateRoleAsync(dbContext, "Member", cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Nickname = nickname.Trim(),
            Name = name.Trim(),
            LastName = lastName.Trim(),
            Email = normalizedEmail,
            Phone = phone,
            Locality = locality,
            About = about,
            IsWorking = true,
        };

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        dbContext.Set<UserRole>().Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });

        var visitanteProfile = await IdentityCatalogSeedService.GetProfileAsync(dbContext, IdentityCatalogSeedService.VisitanteProfile, cancellationToken);
        dbContext.Set<UserProfileHistory>().Add(new UserProfileHistory { UserId = user.Id, ProfileId = visitanteProfile.Id, IsCurrent = true });
        dbContext.Set<Membership>().Add(new Membership { UserId = user.Id, Status = MembershipStatus.Activa, ActivatedAt = DateTime.UtcNow, Origin = "alta-manual-staff" });

        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public static async Task<User> UpdateAsync(
        TechRidersDbContext dbContext, Guid id, string name, string lastName, string email,
        string? phone, string? locality, string? about, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        var normalizedEmail = email.Trim();
        if (!string.Equals(user.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase)
            && await dbContext.Users.AnyAsync(u => u.Id != id && u.Email == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe otra cuenta con ese correo.");
        }

        user.Name = name.Trim();
        user.LastName = lastName.Trim();
        user.Email = normalizedEmail;
        user.Phone = phone;
        user.Locality = locality;
        user.About = about;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public static Task<User> ActivateAsync(TechRidersDbContext dbContext, Guid id, CancellationToken cancellationToken = default) =>
        SetMembershipStatusAsync(dbContext, id, MembershipStatus.Activa, cancellationToken);

    public static Task<User> DeactivateAsync(TechRidersDbContext dbContext, Guid id, CancellationToken cancellationToken = default) =>
        SetMembershipStatusAsync(dbContext, id, MembershipStatus.Suspendida, cancellationToken);

    private static async Task<User> SetMembershipStatusAsync(TechRidersDbContext dbContext, Guid id, MembershipStatus status, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.Include(u => u.Membership).FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        if (user.Membership is null)
        {
            user.Membership = new Membership { UserId = user.Id, Status = status };
            dbContext.Set<Membership>().Add(user.Membership);
        }
        else
        {
            user.Membership.Status = status;
        }

        if (status == MembershipStatus.Activa)
        {
            user.Membership.ActivatedAt = DateTime.UtcNow;
        }
        else if (status == MembershipStatus.Suspendida)
        {
            user.Membership.SuspendedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public static async Task RevokeRoleAsync(TechRidersDbContext dbContext, Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        var userRole = await dbContext.Set<UserRole>()
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == roleName, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no tiene ese rol asignado.");

        dbContext.Set<UserRole>().Remove(userRole);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task<UserActivitySummary> GetActivityAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default)
    {
        var eventsCount = await dbContext.Set<EventRegistration>().CountAsync(r => r.UserId == userId, cancellationToken);
        var sessionsCount = await dbContext.Set<SessionRegistration>().CountAsync(r => r.UserId == userId, cancellationToken);
        var speakerCount = await dbContext.Set<SessionSpeaker>().CountAsync(s => s.UserId == userId, cancellationToken);
        var fpTourCount = await dbContext.Set<FPTour>().CountAsync(f => f.AmbassadorUserId == userId, cancellationToken);
        var recentActions = await dbContext.Set<IntranetAuditLog>()
            .Where(a => a.ActorUserId == userId)
            .OrderByDescending(a => a.CreatedUtc)
            .Take(20)
            .ToListAsync(cancellationToken);

        return new UserActivitySummary(eventsCount, sessionsCount, speakerCount, fpTourCount, recentActions);
    }

    private static async Task<Role> GetOrCreateRoleAsync(TechRidersDbContext dbContext, string roleName, CancellationToken cancellationToken)
    {
        var role = await dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = roleName, Description = "Rol base de comunidad." };
            await dbContext.Set<Role>().AddAsync(role, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return role;
    }
}

public sealed record UserActivitySummary(int EventsRegistered, int SessionsRegistered, int SpeakerSessions, int FPToursAsAmbassador, List<IntranetAuditLog> RecentActions);
