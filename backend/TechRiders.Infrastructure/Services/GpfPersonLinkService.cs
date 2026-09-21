using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Vinculacion manual y opcional de una persona Tech Riders con GPF (CodUnico). Sin matching automatico (Relaciones GPF §4.4).</summary>
public sealed class GpfPersonLinkService : IGpfPersonLinkService
{
    private readonly TechRidersDbContext _dbContext;

    public GpfPersonLinkService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GpfPersonLink> LinkAsync(Guid userId, string codUnico, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var normalizedCodUnico = codUnico.Trim();
        if (string.IsNullOrWhiteSpace(normalizedCodUnico))
        {
            throw new ArgumentException("CodUnico es obligatorio.");
        }

        var userHasActiveLink = await _dbContext.Set<GpfPersonLink>()
            .AnyAsync(l => l.UserId == userId && l.Status == GpfLinkStatus.Activo, cancellationToken);
        if (userHasActiveLink)
        {
            throw new InvalidOperationException("Esta persona ya tiene un vinculo GPF activo.");
        }

        var codUnicoInUse = await _dbContext.Set<GpfPersonLink>()
            .AnyAsync(l => l.CodUnico == normalizedCodUnico && l.Status == GpfLinkStatus.Activo, cancellationToken);
        if (codUnicoInUse)
        {
            throw new InvalidOperationException("Ese CodUnico ya esta vinculado activamente a otra persona.");
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

        await _dbContext.Set<GpfPersonLink>().AddAsync(link, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return link;
    }

    public async Task UnlinkAsync(Guid linkId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var link = await _dbContext.Set<GpfPersonLink>().FirstOrDefaultAsync(l => l.Id == linkId, cancellationToken)
            ?? throw new InvalidOperationException("Vinculo no encontrado.");

        if (link.Status != GpfLinkStatus.Activo)
        {
            throw new InvalidOperationException("El vinculo ya esta inactivo.");
        }

        link.Status = GpfLinkStatus.Inactivo;
        link.ValidatedByUserId = validatedByUserId;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<GpfPersonLink>> ListAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<GpfPersonLink>()
            .Include(l => l.User)
            .OrderByDescending(l => l.LinkedAt)
            .ToListAsync(cancellationToken);

    public Task<GpfPersonLink?> GetActiveForUserAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<GpfPersonLink>()
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Status == GpfLinkStatus.Activo, cancellationToken);
}
