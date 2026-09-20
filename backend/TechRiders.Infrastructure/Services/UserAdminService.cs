using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Gestion completa de usuarios (alta, edicion, activacion/baja, roles) para el panel de Staff/Admin.</summary>
public sealed class UserAdminService : IUserAdminService
{
    private readonly TechRidersDbContext _dbContext;

    public UserAdminService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(List<User> Items, int TotalCount)> ListAsync(
        string? search, string? role, string? membershipStatus,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users
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

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Users
            .Include(u => u.Membership)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Capabilities).ThenInclude(c => c.Capability)
            .Include(u => u.ProfileHistories).ThenInclude(ph => ph.Profile)
            .Include(u => u.OrganizationRelations).ThenInclude(po => po.Organization)
            .Include(u => u.UserSkills).ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User> CreateAsync(
        string nickname, string name, string lastName, string email,
        string? phone, string? locality, string? about, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        if (await _dbContext.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe una cuenta registrada con ese correo.");
        }

        var memberRole = await GetOrCreateRoleAsync("Member", cancellationToken);

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

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _dbContext.Set<UserRole>().Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });

        var visitanteProfile = await IdentityCatalogSeedService.GetProfileAsync(_dbContext, IdentityCatalogSeedService.VisitanteProfile, cancellationToken);
        _dbContext.Set<UserProfileHistory>().Add(new UserProfileHistory { UserId = user.Id, ProfileId = visitanteProfile.Id, IsCurrent = true });
        _dbContext.Set<Membership>().Add(new Membership { UserId = user.Id, Status = MembershipStatus.Activa, ActivatedAt = DateTime.UtcNow, Origin = "alta-manual-staff" });

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(
        Guid id, string name, string lastName, string email,
        string? phone, string? locality, string? about, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        var normalizedEmail = email.Trim();
        if (!string.Equals(user.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase)
            && await _dbContext.Users.AnyAsync(u => u.Id != id && u.Email == normalizedEmail, cancellationToken))
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

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public Task<User> ActivateAsync(Guid id, CancellationToken cancellationToken = default) =>
        SetMembershipStatusAsync(id, MembershipStatus.Activa, cancellationToken);

    public Task<User> DeactivateAsync(Guid id, CancellationToken cancellationToken = default) =>
        SetMembershipStatusAsync(id, MembershipStatus.Suspendida, cancellationToken);

    private async Task<User> SetMembershipStatusAsync(Guid id, MembershipStatus status, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Include(u => u.Membership).FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        if (user.Membership is null)
        {
            user.Membership = new Membership { UserId = user.Id, Status = status };
            _dbContext.Set<Membership>().Add(user.Membership);
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

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task RevokeRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        var userRole = await _dbContext.Set<UserRole>()
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == roleName, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no tiene ese rol asignado.");

        _dbContext.Set<UserRole>().Remove(userRole);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserActivitySummary> GetActivityAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var eventsCount = await _dbContext.Set<EventRegistration>().CountAsync(r => r.UserId == userId, cancellationToken);
        var sessionsCount = await _dbContext.Set<SessionRegistration>().CountAsync(r => r.UserId == userId, cancellationToken);
        var speakerCount = await _dbContext.Set<SessionSpeaker>().CountAsync(s => s.UserId == userId, cancellationToken);
        var fpTourCount = await _dbContext.Set<FPTour>().CountAsync(f => f.AmbassadorUserId == userId, cancellationToken);
        var recentActions = await _dbContext.Set<IntranetAuditLog>()
            .Where(a => a.ActorUserId == userId)
            .OrderByDescending(a => a.CreatedUtc)
            .Take(20)
            .ToListAsync(cancellationToken);

        return new UserActivitySummary(eventsCount, sessionsCount, speakerCount, fpTourCount, recentActions);
    }

    private async Task<Role> GetOrCreateRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = roleName, Description = "Rol base de comunidad." };
            await _dbContext.Set<Role>().AddAsync(role, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return role;
    }
}
