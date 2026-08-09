using TechRiders.Api.Contracts.Requests.Engagement;

namespace TechRiders.Api.Services;

public interface IPublicEngagementIntakeService
{
    PublicEngagementIntakeResult ProcessJoinRequest(JoinRequest request);
}

public sealed class PublicEngagementIntakeService : IPublicEngagementIntakeService
{
    private static readonly HashSet<string> AllowedRequestTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "member",
        "ambassador",
        "session",
    };

    private readonly IMvpRuntimeStateStore _mvpRuntimeStateStore;

    public PublicEngagementIntakeService(IMvpRuntimeStateStore mvpRuntimeStateStore)
    {
        _mvpRuntimeStateStore = mvpRuntimeStateStore ?? throw new ArgumentNullException(nameof(mvpRuntimeStateStore));
    }

    public PublicEngagementIntakeResult ProcessJoinRequest(JoinRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!AllowedRequestTypes.Contains(request.RequestType))
        {
            throw new InvalidOperationException("RequestType must be member, ambassador or session.");
        }

        if (string.Equals(request.RequestType, "member", StringComparison.OrdinalIgnoreCase))
        {
            _mvpRuntimeStateStore.UpsertMemberProfile(request.Email, new MemberProfileState
            {
                Name = request.Name,
                Email = request.Email,
                Bio = request.Motivation,
                Interests = request.Audience ?? string.Empty,
                Audience = request.Audience ?? "junior",
                CommunityRole = request.CommunityRole,
                Organization = request.Organization ?? string.Empty,
            });
        }

        if (string.Equals(request.RequestType, "ambassador", StringComparison.OrdinalIgnoreCase))
        {
            _mvpRuntimeStateStore.UpsertAmbassadorPortal(request.Email, new AmbassadorPortalState
            {
                Email = request.Email,
                Bio = request.Motivation,
                Specialties = string.Join(" · ", new[] { request.Audience, request.Organization }.Where(value => !string.IsNullOrWhiteSpace(value))),
                Availability = "Pendiente de completar por el ambassador en intranet",
            });
        }

        return new PublicEngagementIntakeResult
        {
            Message = request.RequestType switch
            {
                "member" => "Member application received",
                "ambassador" => "Ambassador application received",
                "session" => "Session request received",
                _ => "Request received",
            },
        };
    }
}

public sealed class PublicEngagementIntakeResult
{
    public bool Success { get; init; } = true;

    public string Message { get; init; } = string.Empty;
}