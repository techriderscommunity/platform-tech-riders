using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>
/// Agrega los distintos "pending" ya existentes (capacidades, relaciones organizacion, comuneras,
/// centros publicos) en una unica bandeja de lectura. No introduce logica de negocio nueva:
/// cada tipo se resuelve llamando al endpoint especifico correspondiente (approve/reject).
/// </summary>
public sealed class ApprovalsService : IApprovalsService
{
    private readonly TechRidersDbContext _dbContext;

    public ApprovalsService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApprovalItem>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        var items = new List<ApprovalItem>();

        var capabilityRequests = await _dbContext.Set<Domain.Entities.UserCapability>()
            .Include(uc => uc.User)
            .Include(uc => uc.Capability)
            .Where(uc => uc.Status == CapabilityStatus.Pendiente)
            .ToListAsync(cancellationToken);
        items.AddRange(capabilityRequests.Select(uc => new ApprovalItem(
            uc.Id, "CapabilityRequest",
            $"{uc.User?.Name} {uc.User?.LastName} → {uc.Capability.Name}",
            uc.User?.Email, uc.RequestedAt, "Usuarios/Ambassadors")));

        var organizationRelations = await _dbContext.Set<Domain.Entities.PersonOrganization>()
            .Include(po => po.User)
            .Include(po => po.Organization)
            .Where(po => po.Status == PersonOrganizationStatus.Pendiente)
            .ToListAsync(cancellationToken);
        items.AddRange(organizationRelations.Select(po => new ApprovalItem(
            po.Id, "OrganizationRelation",
            $"{po.User?.Name} {po.User?.LastName} → {po.Organization?.Name}",
            po.User?.Email, po.CreatedAt, "Centros/Organizaciones")));

        var communityPartnerApplications = await _dbContext.Set<Domain.Entities.CommunityPartnerApplication>()
            .Where(a => a.Status == "pending")
            .ToListAsync(cancellationToken);
        items.AddRange(communityPartnerApplications.Select(a => new ApprovalItem(
            a.Id, "CommunityPartnerApplication", a.Name, a.ContactEmail, a.CreatedAt, "Comuneras")));

        var pendingOrganizations = await _dbContext.Set<Domain.Entities.Organization>()
            .Where(o => !o.IsActive && o.Origin == "SolicitudPublica")
            .ToListAsync(cancellationToken);
        items.AddRange(pendingOrganizations.Select(o => new ApprovalItem(
            o.Id, "OrganizationRequest", o.Name, null, o.CreatedAt, "Centros")));

        return items.OrderBy(i => i.RequestedAt).ToList();
    }

    public async Task<Dictionary<string, int>> GetPendingCountsByTypeAsync(CancellationToken cancellationToken = default)
    {
        var items = await GetPendingAsync(cancellationToken);
        return items.GroupBy(i => i.Type).ToDictionary(g => g.Key, g => g.Count());
    }
}
