using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

public sealed record ApprovalItem(Guid Id, string Type, string Title, string? RequestedBy, DateTime RequestedAt, string Module);

/// <summary>
/// Agrega los distintos "pending" ya existentes (capacidades, relaciones organizaci\u00f3n, comuneras,
/// centros p\u00fablicos) en una \u00fanica bandeja de lectura. No introduce l\u00f3gica de negocio nueva:
/// cada tipo se resuelve llamando al endpoint espec\u00edfico correspondiente (approve/reject).
/// </summary>
public static class ApprovalsAggregationService
{
    public static async Task<List<ApprovalItem>> GetPendingAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var items = new List<ApprovalItem>();

        var capabilityRequests = await dbContext.Set<Domain.Entities.UserCapability>()
            .Include(uc => uc.User)
            .Include(uc => uc.Capability)
            .Where(uc => uc.Status == CapabilityStatus.Pendiente)
            .ToListAsync(cancellationToken);
        items.AddRange(capabilityRequests.Select(uc => new ApprovalItem(
            uc.Id, "CapabilityRequest",
            $"{uc.User?.Name} {uc.User?.LastName} \u2192 {uc.Capability.Name}",
            uc.User?.Email, uc.RequestedAt, "Usuarios/Ambassadors")));

        var organizationRelations = await dbContext.Set<Domain.Entities.PersonOrganization>()
            .Include(po => po.User)
            .Include(po => po.Organization)
            .Where(po => po.Status == PersonOrganizationStatus.Pendiente)
            .ToListAsync(cancellationToken);
        items.AddRange(organizationRelations.Select(po => new ApprovalItem(
            po.Id, "OrganizationRelation",
            $"{po.User?.Name} {po.User?.LastName} \u2192 {po.Organization?.Name}",
            po.User?.Email, po.CreatedAt, "Centros/Organizaciones")));

        var communityPartnerApplications = await dbContext.Set<Domain.Entities.CommunityPartnerApplication>()
            .Where(a => a.Status == "pending")
            .ToListAsync(cancellationToken);
        items.AddRange(communityPartnerApplications.Select(a => new ApprovalItem(
            a.Id, "CommunityPartnerApplication", a.Name, a.ContactEmail, a.CreatedAt, "Comuneras")));

        var pendingOrganizations = await dbContext.Set<Domain.Entities.Organization>()
            .Where(o => !o.IsActive && o.Origin == "SolicitudPublica")
            .ToListAsync(cancellationToken);
        items.AddRange(pendingOrganizations.Select(o => new ApprovalItem(
            o.Id, "OrganizationRequest", o.Name, null, o.CreatedAt, "Centros")));

        return items.OrderBy(i => i.RequestedAt).ToList();
    }

    public static async Task<Dictionary<string, int>> GetPendingCountsByTypeAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var items = await GetPendingAsync(dbContext, cancellationToken);
        return items.GroupBy(i => i.Type).ToDictionary(g => g.Key, g => g.Count());
    }
}
