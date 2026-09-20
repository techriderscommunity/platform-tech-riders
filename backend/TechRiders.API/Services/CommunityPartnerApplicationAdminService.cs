using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>
/// Aprobación/rechazo de solicitudes de Comunera (CommunityPartnerApplication) por Admin/Staff.
/// Al aprobar, crea la Organization (EntidadColaboradora) y la Community asociada, reutilizando
/// el modelo ya existente en vez de duplicar datos.
/// </summary>
public static class CommunityPartnerApplicationAdminService
{
    private const string PendingStatus = "pending";
    private const string ApprovedStatus = "approved";
    private const string RejectedStatus = "rejected";

    public static Task<List<CommunityPartnerApplication>> GetPendingAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<CommunityPartnerApplication>()
            .Where(a => a.Status == PendingStatus)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

    public static async Task<CommunityPartnerApplication> ApproveAsync(TechRidersDbContext dbContext, Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var application = await LoadPendingAsync(dbContext, applicationId, cancellationToken);

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OrganizationType = OrganizationType.EntidadColaboradora,
            Name = application.Name,
            Website = application.Website,
            Notes = application.Motivation,
            Origin = "ComuneraAprobada",
            IsActive = true,
        };
        await dbContext.Set<Organization>().AddAsync(organization, cancellationToken);

        var community = new Community
        {
            Id = Guid.NewGuid(),
            Name = application.Name,
            Description = application.WhatYouDo,
            Website = application.Website,
            LogoUrl = application.LogoUrl,
            LinkedIn = application.LinkedIn,
            Instagram = application.Instagram,
            X = application.X,
            YouTube = application.YouTube,
            Github = application.Github,
        };
        await dbContext.Set<Community>().AddAsync(community, cancellationToken);

        application.Status = ApprovedStatus;
        application.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return application;
    }

    public static async Task<CommunityPartnerApplication> RejectAsync(TechRidersDbContext dbContext, Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var application = await LoadPendingAsync(dbContext, applicationId, cancellationToken);
        application.Status = RejectedStatus;
        application.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return application;
    }

    private static async Task<CommunityPartnerApplication> LoadPendingAsync(TechRidersDbContext dbContext, Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await dbContext.Set<CommunityPartnerApplication>()
            .FirstOrDefaultAsync(a => a.Id == applicationId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud de comunera no encontrada.");

        if (application.Status != PendingStatus)
        {
            throw new InvalidOperationException("La solicitud ya fue resuelta.");
        }

        return application;
    }
}
