using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Organizaciones y su relacion con personas (Requisitos Arquitectura §6). Una organizacion no es una persona ni un rol.</summary>
public sealed class OrganizationService : IOrganizationService
{
    private readonly TechRidersDbContext _dbContext;

    public OrganizationService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Organization> CreateAsync(string organizationType, string name, string? taxId, string? website, string? address, string? province, string? notes = null, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrganizationType>(organizationType, ignoreCase: true, out var parsedType))
        {
            throw new ArgumentException($"Tipo de organizacion '{organizationType}' no reconocido.");
        }

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OrganizationType = parsedType,
            Name = name.Trim(),
            TaxId = taxId,
            Website = website,
            Address = address,
            Province = province,
            Notes = notes,
            Origin = "staff-admin",
        };

        await _dbContext.Set<Organization>().AddAsync(organization, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public async Task<Organization> CreatePendingAsync(string organizationType, string name, string? contactInfo, string? website, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrganizationType>(organizationType, ignoreCase: true, out var parsedType))
        {
            throw new ArgumentException($"Tipo de organizacion '{organizationType}' no reconocido.");
        }

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OrganizationType = parsedType,
            Name = name.Trim(),
            Website = website,
            Notes = contactInfo,
            Origin = "SolicitudPublica",
            IsActive = false,
        };

        await _dbContext.Set<Organization>().AddAsync(organization, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public Task<List<Organization>> ListAsync(string? organizationType = null, bool onlyActive = true, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Organization>().AsQueryable();
        if (onlyActive)
        {
            query = query.Where(o => o.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(organizationType) && Enum.TryParse<OrganizationType>(organizationType, ignoreCase: true, out var parsedType))
        {
            query = query.Where(o => o.OrganizationType == parsedType);
        }

        return query.OrderBy(o => o.Name).ToListAsync(cancellationToken);
    }

    public Task<List<Organization>> GetPendingAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<Organization>()
            .Where(o => !o.IsActive && o.Origin == "SolicitudPublica")
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<Organization> UpdateAsync(Guid id, string name, string? taxId, string? website, string? address, string? province, string? notes, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Set<Organization>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("La organizacion indicada no existe.");

        organization.Name = name.Trim();
        organization.TaxId = taxId;
        organization.Website = website;
        organization.Address = address;
        organization.Province = province;
        organization.Notes = notes;
        organization.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public Task<Organization> ActivateAsync(Guid id, CancellationToken cancellationToken = default) =>
        SetActiveAsync(id, true, cancellationToken);

    public Task<Organization> SuspendAsync(Guid id, CancellationToken cancellationToken = default) =>
        SetActiveAsync(id, false, cancellationToken);

    private async Task<Organization> SetActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var organization = await _dbContext.Set<Organization>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("La organizacion indicada no existe.");

        organization.IsActive = isActive;
        organization.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public async Task<PersonOrganization> RequestRelationAsync(Guid userId, Guid organizationId, string relationType, string? position, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PersonOrganizationRelationType>(relationType, ignoreCase: true, out var parsedRelation))
        {
            throw new ArgumentException($"Tipo de relacion '{relationType}' no reconocido.");
        }

        var organizationExists = await _dbContext.Set<Organization>().AnyAsync(o => o.Id == organizationId, cancellationToken);
        if (!organizationExists)
        {
            throw new InvalidOperationException("La organizacion indicada no existe.");
        }

        var alreadyOpen = await _dbContext.Set<PersonOrganization>()
            .AnyAsync(po => po.UserId == userId && po.OrganizationId == organizationId
                && (po.Status == PersonOrganizationStatus.Pendiente || po.Status == PersonOrganizationStatus.Activa), cancellationToken);
        if (alreadyOpen)
        {
            throw new InvalidOperationException("Ya existe una relacion pendiente o activa con esta organizacion.");
        }

        var relation = new PersonOrganization
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrganizationId = organizationId,
            RelationType = parsedRelation,
            Position = position,
            Status = PersonOrganizationStatus.Pendiente,
        };

        await _dbContext.Set<PersonOrganization>().AddAsync(relation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    public Task<List<PersonOrganization>> GetPendingRelationsAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<PersonOrganization>()
            .Include(po => po.User)
            .Include(po => po.Organization)
            .Where(po => po.Status == PersonOrganizationStatus.Pendiente)
            .OrderBy(po => po.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<PersonOrganization> ApproveRelationAsync(Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var relation = await LoadPendingRelationAsync(relationId, cancellationToken);
        relation.Status = PersonOrganizationStatus.Activa;
        relation.StartDate = DateTime.UtcNow;
        relation.ValidatedByUserId = validatedByUserId;
        relation.ValidatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    public async Task<PersonOrganization> RejectRelationAsync(Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var relation = await LoadPendingRelationAsync(relationId, cancellationToken);
        relation.Status = PersonOrganizationStatus.Rechazada;
        relation.ValidatedByUserId = validatedByUserId;
        relation.ValidatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    private async Task<PersonOrganization> LoadPendingRelationAsync(Guid relationId, CancellationToken cancellationToken)
    {
        var relation = await _dbContext.Set<PersonOrganization>()
            .Include(po => po.Organization)
            .FirstOrDefaultAsync(po => po.Id == relationId, cancellationToken)
            ?? throw new InvalidOperationException("Relacion no encontrada.");

        if (relation.Status != PersonOrganizationStatus.Pendiente)
        {
            throw new InvalidOperationException("La relacion ya fue resuelta.");
        }

        return relation;
    }
}
