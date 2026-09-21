using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Consentimientos por finalidad (Requisitos Arquitectura §8). Un interes/preferencia NUNCA es prueba de consentimiento.</summary>
public sealed class ConsentService : IConsentService
{
    private readonly TechRidersDbContext _dbContext;

    public ConsentService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<ConsentPurpose>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<ConsentPurpose>().Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync(cancellationToken);

    public Task<List<Consent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<Consent>()
            .Include(c => c.Purpose)
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<Consent> GrantAsync(Guid userId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var consent = await GetOrCreateAsync(userId, purposeCode, cancellationToken);
        consent.Status = ConsentStatus.Otorgado;
        consent.GrantedAt = DateTime.UtcNow;
        consent.WithdrawnAt = null;
        consent.Origin = "member-self-service";
        await _dbContext.SaveChangesAsync(cancellationToken);
        return consent;
    }

    public async Task<Consent> WithdrawAsync(Guid userId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var consent = await GetOrCreateAsync(userId, purposeCode, cancellationToken);
        consent.Status = ConsentStatus.Retirado;
        consent.WithdrawnAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return consent;
    }

    private async Task<Consent> GetOrCreateAsync(Guid userId, string purposeCode, CancellationToken cancellationToken)
    {
        var purpose = await _dbContext.Set<ConsentPurpose>().FirstOrDefaultAsync(p => p.Code == purposeCode, cancellationToken)
            ?? throw new InvalidOperationException($"Finalidad '{purposeCode}' no reconocida.");

        var consent = await _dbContext.Set<Consent>()
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

        await _dbContext.Set<Consent>().AddAsync(consent, cancellationToken);
        return consent;
    }
}
