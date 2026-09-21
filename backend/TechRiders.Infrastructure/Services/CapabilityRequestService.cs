using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>
/// Flujo de "ascenso" de Member a un rol de comunidad (Staff, Community Leader, Ambassador, Center,
/// Community Partner) con aprobacion de Admin/Staff. Member es automatico y no pasa por aqui.
/// </summary>
public sealed class CapabilityRequestService : ICapabilityRequestService
{
    private readonly TechRidersDbContext _dbContext;

    public CapabilityRequestService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserCapability> RequestAsync(Guid userId, string capabilityName, CancellationToken cancellationToken = default)
    {
        var normalizedName = capabilityName.Trim();
        var capability = await _dbContext.Set<Capability>().FirstOrDefaultAsync(c => c.Name == normalizedName, cancellationToken)
            ?? throw new InvalidOperationException($"'{normalizedName}' no es un rol solicitable del catalogo.");

        var alreadyOpen = await _dbContext.Set<UserCapability>()
            .AnyAsync(uc => uc.UserId == userId && uc.CapabilityId == capability.Id
                && (uc.Status == CapabilityStatus.Pendiente || uc.Status == CapabilityStatus.Activa), cancellationToken);

        if (alreadyOpen)
        {
            throw new InvalidOperationException("Ya tienes una solicitud pendiente o activa para este rol.");
        }

        var request = new UserCapability
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CapabilityId = capability.Id,
            Status = CapabilityStatus.Pendiente,
            RequestedAt = DateTime.UtcNow,
        };

        await _dbContext.Set<UserCapability>().AddAsync(request, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public Task<List<UserCapability>> GetPendingAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<UserCapability>()
            .Include(uc => uc.User)
            .Include(uc => uc.Capability)
            .Where(uc => uc.Status == CapabilityStatus.Pendiente)
            .OrderBy(uc => uc.RequestedAt)
            .ToListAsync(cancellationToken);

    public Task<List<UserCapability>> GetHistoryAsync(string? capabilityName, CapabilityStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<UserCapability>()
            .Include(uc => uc.User)
            .Include(uc => uc.Capability)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(capabilityName))
        {
            query = query.Where(uc => uc.Capability.Name == capabilityName);
        }

        if (status.HasValue)
        {
            query = query.Where(uc => uc.Status == status.Value);
        }

        return query.OrderByDescending(uc => uc.RequestedAt).ToListAsync(cancellationToken);
    }

    public async Task<UserCapability> RevokeAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await _dbContext.Set<UserCapability>()
            .Include(uc => uc.Capability)
            .FirstOrDefaultAsync(uc => uc.Id == requestId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        if (request.Status != CapabilityStatus.Activa)
        {
            throw new InvalidOperationException("Solo se puede revocar una capacidad activa.");
        }

        request.Status = CapabilityStatus.Revocada;
        request.ValidatedAt = DateTime.UtcNow;
        request.ValidatedByUserId = validatedByUserId;

        var role = await _dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == request.Capability.Name, cancellationToken);
        if (role is not null)
        {
            var userRole = await _dbContext.Set<UserRole>().FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id, cancellationToken);
            if (userRole is not null)
            {
                _dbContext.Set<UserRole>().Remove(userRole);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public async Task<UserCapability> ApproveAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await LoadPendingAsync(requestId, cancellationToken);

        request.Status = CapabilityStatus.Activa;
        request.ValidatedAt = DateTime.UtcNow;
        request.ValidFrom = DateTime.UtcNow;
        request.ValidatedByUserId = validatedByUserId;

        var role = await _dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == request.Capability.Name, cancellationToken);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = request.Capability.Name, Description = "Rol otorgado por solicitud de capacidad aprobada." };
            await _dbContext.Set<Role>().AddAsync(role, cancellationToken);
        }

        var hasRole = await _dbContext.Set<UserRole>().AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id, cancellationToken);
        if (!hasRole)
        {
            await _dbContext.Set<UserRole>().AddAsync(new UserRole { UserId = request.UserId, RoleId = role.Id }, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public async Task<UserCapability> RejectAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await LoadPendingAsync(requestId, cancellationToken);

        request.Status = CapabilityStatus.Rechazada;
        request.ValidatedAt = DateTime.UtcNow;
        request.ValidatedByUserId = validatedByUserId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    private async Task<UserCapability> LoadPendingAsync(Guid requestId, CancellationToken cancellationToken)
    {
        var request = await _dbContext.Set<UserCapability>()
            .Include(uc => uc.Capability)
            .FirstOrDefaultAsync(uc => uc.Id == requestId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        if (request.Status != CapabilityStatus.Pendiente)
        {
            throw new InvalidOperationException("La solicitud ya fue resuelta.");
        }

        return request;
    }
}
