using TechRiders.Application.DTOs.Requests.CommunityPartner;
using TechRiders.Application.DTOs.Responses.CommunityPartner;
using TechRiders.Application.Interfaces;
using TechRiders.Application.Social;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class CommunityPartnerApplicationService : ICommunityPartnerApplicationService
{
    private const string PendingStatus = "pending";
    private readonly IUnitOfWork unitOfWork;

    public CommunityPartnerApplicationService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<CommunityPartnerApplicationResponse> CreateAsync(
        CreateCommunityPartnerApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var website = NormalizeRequired(request.Website, nameof(request.Website));
        var contactEmail = NormalizeRequired(request.ContactEmail, nameof(request.ContactEmail)).ToLowerInvariant();

        if (await unitOfWork.CommunityPartnerApplications.ExistsDuplicateAsync(name, website, contactEmail, cancellationToken))
        {
            throw new InvalidOperationException("A community partner application already exists for these details.");
        }

        var application = new CommunityPartnerApplication
        {
            Id = Guid.NewGuid(),
            Name = name,
            Website = website,
            LogoUrl = NormalizeOptional(request.LogoUrl),
            ContactEmail = contactEmail,
            ContactName = NormalizeRequired(request.ContactName, nameof(request.ContactName)),
            WhoYouAre = NormalizeRequired(request.WhoYouAre, nameof(request.WhoYouAre)),
            WhatYouDo = NormalizeRequired(request.WhatYouDo, nameof(request.WhatYouDo)),
            Mission = NormalizeRequired(request.Mission, nameof(request.Mission)),
            Topics = NormalizeRequired(request.Topics, nameof(request.Topics)),
            Scope = NormalizeRequired(request.Scope, nameof(request.Scope)).ToLowerInvariant(),
            LinkedIn = SocialProfileIdentifier.Normalize(request.LinkedIn),
            Instagram = SocialProfileIdentifier.Normalize(request.Instagram),
            X = SocialProfileIdentifier.Normalize(request.X),
            YouTube = SocialProfileIdentifier.Normalize(request.YouTube),
            Github = SocialProfileIdentifier.Normalize(request.Github),
            Motivation = NormalizeRequired(request.Motivation, nameof(request.Motivation)),
            CollaborationIdeas = NormalizeRequired(request.CollaborationIdeas, nameof(request.CollaborationIdeas)),
            Status = PendingStatus,
        };

        await unitOfWork.CommunityPartnerApplications.AddAsync(application, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommunityPartnerApplicationResponse(
            application.Id,
            application.Status,
            SocialProfileUrls.Build(SocialProfileUrls.LinkedInCompany, application.LinkedIn),
            SocialProfileUrls.Build(SocialProfileUrls.Instagram, application.Instagram),
            SocialProfileUrls.Build(SocialProfileUrls.X, application.X),
            SocialProfileUrls.Build(SocialProfileUrls.YouTube, application.YouTube),
            SocialProfileUrls.Build(SocialProfileUrls.GitHub, application.Github));
    }

    private static string NormalizeRequired(string value, string parameterName)
    {
        var normalized = value.Trim();
        return normalized.Length > 0
            ? normalized
            : throw new ArgumentException("A value is required.", parameterName);
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    public async Task<List<CommunityPartnerApplication>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        var pending = await unitOfWork.CommunityPartnerApplications.FindAsync(a => a.Status == PendingStatus, cancellationToken);
        return pending.OrderBy(a => a.CreatedAt).ToList();
    }

    public async Task<CommunityPartnerApplication> ApproveAsync(Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var application = await LoadPendingAsync(applicationId, cancellationToken);

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OrganizationType = TechRiders.Domain.Enums.OrganizationType.EntidadColaboradora,
            Name = application.Name,
            Website = application.Website,
            Notes = application.Motivation,
            Origin = "ComuneraAprobada",
            IsActive = true,
        };
        await unitOfWork.Organizations.AddAsync(organization, cancellationToken);

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
        await unitOfWork.Communities.AddAsync(community, cancellationToken);

        application.Status = "approved";
        application.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return application;
    }

    public async Task<CommunityPartnerApplication> RejectAsync(Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default)
    {
        var application = await LoadPendingAsync(applicationId, cancellationToken);
        application.Status = "rejected";
        application.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return application;
    }

    private async Task<CommunityPartnerApplication> LoadPendingAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await unitOfWork.CommunityPartnerApplications.GetByIdAsync(applicationId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud de comunera no encontrada.");

        if (application.Status != PendingStatus)
        {
            throw new InvalidOperationException("La solicitud ya fue resuelta.");
        }

        return application;
    }
}