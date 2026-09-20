using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Consentimientos por finalidad (Requisitos Arquitectura §8). Un interes/preferencia NUNCA es prueba de consentimiento.</summary>
public interface IConsentService
{
    Task<List<ConsentPurpose>> GetPurposesAsync(CancellationToken cancellationToken = default);
    Task<List<Consent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Consent> GrantAsync(Guid userId, string purposeCode, CancellationToken cancellationToken = default);
    Task<Consent> WithdrawAsync(Guid userId, string purposeCode, CancellationToken cancellationToken = default);
}
