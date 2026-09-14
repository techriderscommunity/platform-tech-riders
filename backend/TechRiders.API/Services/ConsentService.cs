using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Consentimientos por finalidad (Requisitos Arquitectura §8). Un interés/preferencia NUNCA es prueba de consentimiento.</summary>
public static class ConsentService
{
    public static Task<List<ConsentPurpose>> GetPurposesAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<ConsentPurpose>().Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync(cancellationToken);

    public static Task<List<Consent>> GetUserConsentsAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<Consent>()
            .Include(c => c.Purpose)
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);

    public static async Task<Consent> GrantAsync(TechRidersDbContext dbContext, Guid userId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var consent = await GetOrCreateAsync(dbContext, userId, purposeCode, cancellationToken);
        consent.Status = ConsentStatus.Otorgado;
        consent.GrantedAt = DateTime.UtcNow;
        consent.WithdrawnAt = null;
        consent.Origin = "member-self-service";
        await dbContext.SaveChangesAsync(cancellationToken);
        return consent;
    }

    public static async Task<Consent> WithdrawAsync(TechRidersDbContext dbContext, Guid userId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var consent = await GetOrCreateAsync(dbContext, userId, purposeCode, cancellationToken);
        consent.Status = ConsentStatus.Retirado;
        consent.WithdrawnAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return consent;
    }

    private static async Task<Consent> GetOrCreateAsync(TechRidersDbContext dbContext, Guid userId, string purposeCode, CancellationToken cancellationToken)
    {
        var purpose = await dbContext.Set<ConsentPurpose>().FirstOrDefaultAsync(p => p.Code == purposeCode, cancellationToken)
            ?? throw new InvalidOperationException($"Finalidad '{purposeCode}' no reconocida.");

        var consent = await dbContext.Set<Consent>()
            .FirstOrDefaultAsync(c => c.UserId == userId && c.PurposeId == purpose.Id, cancellationToken);

        if (consent is not null)
        {
            return consent;
        }

        consent = new Consent
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PurposeId = purpose.Id,
            Status = ConsentStatus.Pendiente,
        };

        await dbContext.Set<Consent>().AddAsync(consent, cancellationToken);
        return consent;
    }
}
