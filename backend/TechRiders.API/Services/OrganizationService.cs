using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Organizaciones y su relación con personas (Requisitos Arquitectura §6). Una organización no es una persona ni un rol.</summary>
public static class OrganizationService
{
    public static async Task<Organization> CreateAsync(TechRidersDbContext dbContext, string organizationType, string name, string? taxId, string? website, string? address, string? province, string? notes = null, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrganizationType>(organizationType, ignoreCase: true, out var parsedType))
        {
            throw new ArgumentException($"Tipo de organización '{organizationType}' no reconocido.");
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

        await dbContext.Set<Organization>().AddAsync(organization, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    /// <summary>Crea una organización pendiente de aprobación a partir de una solicitud pública (Centro/Comunera).</summary>
    public static async Task<Organization> CreatePendingAsync(TechRidersDbContext dbContext, string organizationType, string name, string? contactInfo, string? website, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrganizationType>(organizationType, ignoreCase: true, out var parsedType))
        {
            throw new ArgumentException($"Tipo de organización '{organizationType}' no reconocido.");
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

        await dbContext.Set<Organization>().AddAsync(organization, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public static Task<List<Organization>> ListAsync(TechRidersDbContext dbContext, string? organizationType = null, bool onlyActive = true, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Organization>().AsQueryable();
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

    public static Task<List<Organization>> GetPendingAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<Organization>()
            .Where(o => !o.IsActive && o.Origin == "SolicitudPublica")
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

    public static async Task<Organization> UpdateAsync(TechRidersDbContext dbContext, Guid id, string name, string? taxId, string? website, string? address, string? province, string? notes, CancellationToken cancellationToken = default)
    {
        var organization = await dbContext.Set<Organization>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("La organización indicada no existe.");

        organization.Name = name.Trim();
        organization.TaxId = taxId;
        organization.Website = website;
        organization.Address = address;
        organization.Province = province;
        organization.Notes = notes;
        organization.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public static Task<Organization> ActivateAsync(TechRidersDbContext dbContext, Guid id, CancellationToken cancellationToken = default) =>
        SetActiveAsync(dbContext, id, true, cancellationToken);

    public static Task<Organization> SuspendAsync(TechRidersDbContext dbContext, Guid id, CancellationToken cancellationToken = default) =>
        SetActiveAsync(dbContext, id, false, cancellationToken);

    private static async Task<Organization> SetActiveAsync(TechRidersDbContext dbContext, Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var organization = await dbContext.Set<Organization>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            ?? throw new InvalidOperationException("La organización indicada no existe.");

        organization.IsActive = isActive;
        organization.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public static async Task<PersonOrganization> RequestRelationAsync(TechRidersDbContext dbContext, Guid userId, Guid organizationId, string relationType, string? position, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PersonOrganizationRelationType>(relationType, ignoreCase: true, out var parsedRelation))
        {
            throw new ArgumentException($"Tipo de relación '{relationType}' no reconocido.");
        }

        var organizationExists = await dbContext.Set<Organization>().AnyAsync(o => o.Id == organizationId, cancellationToken);
        if (!organizationExists)
        {
            throw new InvalidOperationException("La organización indicada no existe.");
        }

        var alreadyOpen = await dbContext.Set<PersonOrganization>()
            .AnyAsync(po => po.UserId == userId && po.OrganizationId == organizationId
                && (po.Status == PersonOrganizationStatus.Pendiente || po.Status == PersonOrganizationStatus.Activa), cancellationToken);
        if (alreadyOpen)
        {
            throw new InvalidOperationException("Ya existe una relación pendiente o activa con esta organización.");
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

        await dbContext.Set<PersonOrganization>().AddAsync(relation, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    public static Task<List<PersonOrganization>> GetPendingRelationsAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<PersonOrganization>()
            .Include(po => po.User)
            .Include(po => po.Organization)
            .Where(po => po.Status == PersonOrganizationStatus.Pendiente)
            .OrderBy(po => po.CreatedAt)
            .ToListAsync(cancellationToken);

    public static async Task<PersonOrganization> ApproveRelationAsync(TechRidersDbContext dbContext, Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var relation = await LoadPendingRelationAsync(dbContext, relationId, cancellationToken);
        relation.Status = PersonOrganizationStatus.Activa;
        relation.StartDate = DateTime.UtcNow;
        relation.ValidatedByUserId = validatedByUserId;
        relation.ValidatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    public static async Task<PersonOrganization> RejectRelationAsync(TechRidersDbContext dbContext, Guid relationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var relation = await LoadPendingRelationAsync(dbContext, relationId, cancellationToken);
        relation.Status = PersonOrganizationStatus.Rechazada;
        relation.ValidatedByUserId = validatedByUserId;
        relation.ValidatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return relation;
    }

    private static async Task<PersonOrganization> LoadPendingRelationAsync(TechRidersDbContext dbContext, Guid relationId, CancellationToken cancellationToken)
    {
        var relation = await dbContext.Set<PersonOrganization>()
            .Include(po => po.Organization)
            .FirstOrDefaultAsync(po => po.Id == relationId, cancellationToken)
            ?? throw new InvalidOperationException("Relación no encontrada.");

        if (relation.Status != PersonOrganizationStatus.Pendiente)
        {
            throw new InvalidOperationException("La relación ya fue resuelta.");
        }

        return relation;
    }
}
