using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Vinculación manual y opcional de una persona Tech Riders con GPF (CodUnico). Sin matching automático (Relaciones GPF §4.4).</summary>
public static class GpfPersonLinkService
{
    public static async Task<GpfPersonLink> LinkAsync(TechRidersDbContext dbContext, Guid userId, string codUnico, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var normalizedCodUnico = codUnico.Trim();
        if (string.IsNullOrWhiteSpace(normalizedCodUnico))
        {
            throw new ArgumentException("CodUnico es obligatorio.");
        }

        var userHasActiveLink = await dbContext.Set<GpfPersonLink>()
            .AnyAsync(l => l.UserId == userId && l.Status == GpfLinkStatus.Activo, cancellationToken);
        if (userHasActiveLink)
        {
            throw new InvalidOperationException("Esta persona ya tiene un vínculo GPF activo.");
        }

        var codUnicoInUse = await dbContext.Set<GpfPersonLink>()
            .AnyAsync(l => l.CodUnico == normalizedCodUnico && l.Status == GpfLinkStatus.Activo, cancellationToken);
        if (codUnicoInUse)
        {
            throw new InvalidOperationException("Ese CodUnico ya está vinculado activamente a otra persona.");
        }

        var link = new GpfPersonLink
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CodUnico = normalizedCodUnico,
            Status = GpfLinkStatus.Activo,
            LinkedAt = DateTime.UtcNow,
            LinkMethod = "manual-staff",
            ValidatedByUserId = validatedByUserId,
        };

        await dbContext.Set<GpfPersonLink>().AddAsync(link, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return link;
    }

    public static async Task UnlinkAsync(TechRidersDbContext dbContext, Guid linkId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var link = await dbContext.Set<GpfPersonLink>().FirstOrDefaultAsync(l => l.Id == linkId, cancellationToken)
            ?? throw new InvalidOperationException("Vínculo no encontrado.");

        if (link.Status != GpfLinkStatus.Activo)
        {
            throw new InvalidOperationException("El vínculo ya está inactivo.");
        }

        link.Status = GpfLinkStatus.Inactivo;
        link.ValidatedByUserId = validatedByUserId;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static Task<List<GpfPersonLink>> ListAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<GpfPersonLink>()
            .Include(l => l.User)
            .OrderByDescending(l => l.LinkedAt)
            .ToListAsync(cancellationToken);

    public static Task<GpfPersonLink?> GetActiveForUserAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<GpfPersonLink>()
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Status == GpfLinkStatus.Activo, cancellationToken);
}
