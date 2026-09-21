using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Organizaciones y su relacion con personas (Requisitos Arquitectura §6). Una organizacion no es una persona ni un rol.</summary>
public interface IOrganizationService
{
    Task<Organization> CreateAsync(string organizationType, string name, string? taxId, string? website, string? address, string? province, string? notes = null, CancellationToken cancellationToken = default);

    /// <summary>Crea una organizacion pendiente de aprobacion a partir de una solicitud publica (Centro/Comunera).</summary>
    Task<Organization> CreatePendingAsync(string organizationType, string name, string? contactInfo, string? website, CancellationToken cancellationToken = default);

    Task<List<Organization>> ListAsync(string? organizationType = null, bool onlyActive = true, CancellationToken cancellationToken = default);
    Task<List<Organization>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task<Organization> UpdateAsync(Guid id, string name, string? taxId, string? website, string? address, string? province, string? notes, CancellationToken cancellationToken = default);
    Task<Organization> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Organization> SuspendAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PersonOrganization> RequestRelationAsync(Guid userId, Guid organizationId, string relationType, string? position, CancellationToken cancellationToken = default);
    Task<List<PersonOrganization>> GetPendingRelationsAsync(CancellationToken cancellationToken = default);
    Task<PersonOrganization> ApproveRelationAsync(Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default);
    Task<PersonOrganization> RejectRelationAsync(Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default);
}
