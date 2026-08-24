using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class AmbassadorRepository : Repository<User>, IAmbassadorRepository
{
    private const string AmbassadorRoleName = "ambassador";
    private const string AmbassadorRoleNameEs = "embajador";

    public AmbassadorRepository(TechRidersDbContext context) : base(context) { }

    public async Task<IEnumerable<User>> GetActiveAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        return await AmbassadorsQuery()
            .OrderBy(u => u.LastName).ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> SearchAmbassadorsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetActiveAmbassadorsAsync(cancellationToken);
        }

        var normalized = searchTerm.Trim();

        return await AmbassadorsQuery()
            .Where(u => EF.Functions.Like(u.Name, $"%{normalized}%") ||
                        EF.Functions.Like(u.LastName, $"%{normalized}%") ||
                        EF.Functions.Like(u.Email, $"%{normalized}%") ||
                        (u.Nickname != null && EF.Functions.Like(u.Nickname, $"%{normalized}%")))
            .OrderBy(u => u.LastName).ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAmbassadorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var userIdsInCategory = _context.Set<IntranetUserCategory>()
            .Where(uc => uc.Active && uc.CategoryId == categoryId)
            .Select(uc => uc.UserId);

        return await AmbassadorsQuery()
            .Where(u => userIdsInCategory.Contains(u.Id))
            .OrderBy(u => u.LastName).ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        return await AmbassadorsQuery()
            .Where(u => u.IsWorking)
            .OrderBy(u => u.LastName).ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetAmbassadorWithDetailsAsync(Guid ambassadorId, CancellationToken cancellationToken = default)
    {
        return await AmbassadorsQuery()
            .FirstOrDefaultAsync(u => u.Id == ambassadorId, cancellationToken);
    }

    public async Task<bool> IsAmbassadorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await AmbassadorsQuery()
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task EnsureAmbassadorRoleAsync(User user, CancellationToken cancellationToken = default)
    {
        var ambassadorRole = await GetOrCreateAmbassadorRoleAsync(cancellationToken);

        var hasRole = await _context.Set<UserRole>()
            .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == ambassadorRole.Id, cancellationToken);

        if (!hasRole)
        {
            await _context.Set<UserRole>().AddAsync(new UserRole
            {
                UserId = user.Id,
                RoleId = ambassadorRole.Id
            }, cancellationToken);
        }
    }

    public async Task RemoveAmbassadorRoleAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var ambassadorRoleIds = await _context.Set<Role>()
            .Where(r => r.Name.ToLower() == AmbassadorRoleName || r.Name.ToLower() == AmbassadorRoleNameEs)
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (ambassadorRoleIds.Count == 0)
        {
            return;
        }

        var userRoles = await _context.Set<UserRole>()
            .Where(ur => ur.UserId == userId && ambassadorRoleIds.Contains(ur.RoleId))
            .ToListAsync(cancellationToken);

        if (userRoles.Count > 0)
        {
            _context.Set<UserRole>().RemoveRange(userRoles);
        }
    }

    public async Task<int> CountActiveAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        return await AmbassadorsQuery().CountAsync(cancellationToken);
    }

    private IQueryable<User> AmbassadorsQuery()
    {
        return _dbSet
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.UserCategories)
                .ThenInclude(uc => uc.Category)
            .Where(u => u.IsActive &&
                        u.UserRoles.Any(ur => ur.Role.Name.ToLower() == AmbassadorRoleName ||
                                              ur.Role.Name.ToLower() == AmbassadorRoleNameEs));
    }

    private async Task<Role> GetOrCreateAmbassadorRoleAsync(CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name.ToLower() == AmbassadorRoleName || r.Name.ToLower() == AmbassadorRoleNameEs, cancellationToken);

        if (role != null)
        {
            return role;
        }

        role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Ambassador",
            Description = "Users who represent the ambassador profile"
        };

        await _context.Set<Role>().AddAsync(role, cancellationToken);
        return role;
    }
}
