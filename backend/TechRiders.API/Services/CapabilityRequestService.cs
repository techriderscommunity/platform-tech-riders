using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>
/// Flujo de "ascenso" de Member a un rol de comunidad (Staff, Community Leader, Ambassador, Center,
/// Community Partner) con aprobación de Admin/Staff. Member es automático y no pasa por aquí.
/// </summary>
public static class CapabilityRequestService
{
    public static async Task<UserCapability> RequestAsync(TechRidersDbContext dbContext, Guid userId, string capabilityName, CancellationToken cancellationToken = default)
    {
        var normalizedName = capabilityName.Trim();
        var capability = await dbContext.Set<Capability>().FirstOrDefaultAsync(c => c.Name == normalizedName, cancellationToken)
            ?? throw new InvalidOperationException($"'{normalizedName}' no es un rol solicitable del catálogo.");

        var alreadyOpen = await dbContext.Set<UserCapability>()
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

        await dbContext.Set<UserCapability>().AddAsync(request, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public static Task<List<UserCapability>> GetPendingAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<UserCapability>()
            .Include(uc => uc.User)
            .Include(uc => uc.Capability)
            .Where(uc => uc.Status == CapabilityStatus.Pendiente)
            .OrderBy(uc => uc.RequestedAt)
            .ToListAsync(cancellationToken);

    /// <summary>Histórico completo de solicitudes (todas las capacidades y estados), opcionalmente filtrado.</summary>
    public static Task<List<UserCapability>> GetHistoryAsync(TechRidersDbContext dbContext, string? capabilityName, CapabilityStatus? status, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<UserCapability>()
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

    /// <summary>Revoca una capacidad previamente activa (p.ej. dar de baja a un Ambassador) y retira el rol de sistema asociado.</summary>
    public static async Task<UserCapability> RevokeAsync(TechRidersDbContext dbContext, Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await dbContext.Set<UserCapability>()
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

        var role = await dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == request.Capability.Name, cancellationToken);
        if (role is not null)
        {
            var userRole = await dbContext.Set<UserRole>().FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id, cancellationToken);
            if (userRole is not null)
            {
                dbContext.Set<UserRole>().Remove(userRole);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public static async Task<UserCapability> ApproveAsync(TechRidersDbContext dbContext, Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await LoadPendingAsync(dbContext, requestId, cancellationToken);

        request.Status = CapabilityStatus.Activa;
        request.ValidatedAt = DateTime.UtcNow;
        request.ValidFrom = DateTime.UtcNow;
        request.ValidatedByUserId = validatedByUserId;

        // El rol de sistema (Role/UserRole) es lo que usan los guards; se otorga al aprobar la capacidad.
        var role = await dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == request.Capability.Name, cancellationToken);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = request.Capability.Name, Description = "Rol otorgado por solicitud de capacidad aprobada." };
            await dbContext.Set<Role>().AddAsync(role, cancellationToken);
        }

        var hasRole = await dbContext.Set<UserRole>().AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id, cancellationToken);
        if (!hasRole)
        {
            await dbContext.Set<UserRole>().AddAsync(new UserRole { UserId = request.UserId, RoleId = role.Id }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public static async Task<UserCapability> RejectAsync(TechRidersDbContext dbContext, Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var request = await LoadPendingAsync(dbContext, requestId, cancellationToken);

        request.Status = CapabilityStatus.Rechazada;
        request.ValidatedAt = DateTime.UtcNow;
        request.ValidatedByUserId = validatedByUserId;

        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    private static async Task<UserCapability> LoadPendingAsync(TechRidersDbContext dbContext, Guid requestId, CancellationToken cancellationToken)
    {
        var request = await dbContext.Set<UserCapability>()
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
