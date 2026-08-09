using System.Text.Json.Serialization;

namespace TechRiders.Api.Contracts.Responses.PublicContent;

public sealed class PublicContentResponse
{
    [JsonPropertyName("home")]
    public required HomeContentResponse Home { get; init; }

    [JsonPropertyName("events")]
    public required EventsContentResponse Events { get; init; }

    [JsonPropertyName("centers")]
    public required CentersContentResponse Centers { get; init; }

    [JsonPropertyName("companies")]
    public required CompaniesContentResponse Companies { get; init; }

    [JsonPropertyName("opportunities")]
    public required OpportunitiesContentResponse Opportunities { get; init; }

    [JsonPropertyName("womanTech")]
    public required WomanTechContentResponse WomanTech { get; init; }

    [JsonPropertyName("join")]
    public required JoinContentResponse Join { get; init; }

    [JsonPropertyName("orientaTech")]
    public required OrientaTechContentResponse OrientaTech { get; init; }

    [JsonPropertyName("about")]
    public required AboutContentResponse About { get; init; }

    [JsonPropertyName("tutorials")]
    public required TutorialsContentResponse Tutorials { get; init; }

    [JsonPropertyName("intranet")]
    public required IntranetContentResponse Intranet { get; init; }
}

public sealed class HomeContentResponse
{
    [JsonPropertyName("stats")]
    public required IReadOnlyList<MetricItemResponse> Stats { get; init; }

    [JsonPropertyName("profilePanelCards")]
    public required IReadOnlyList<HomeProfileCardResponse> ProfilePanelCards { get; init; }

    [JsonPropertyName("pastEventPhotos")]
    public required IReadOnlyList<HomePastEventPhotoResponse> PastEventPhotos { get; init; }
}

public sealed class EventsContentResponse
{
    [JsonPropertyName("participationModes")]
    public required IReadOnlyList<ParticipationModeResponse> ParticipationModes { get; init; }

    [JsonPropertyName("galleryGroups")]
    public required IReadOnlyList<GalleryGroupResponse> GalleryGroups { get; init; }

    [JsonPropertyName("talksFallback")]
    public required IReadOnlyList<VideoCarouselItemResponse> TalksFallback { get; init; }
}

public sealed class CentersContentResponse
{
    [JsonPropertyName("metrics")]
    public required IReadOnlyList<MetricItemResponse> Metrics { get; init; }

    [JsonPropertyName("cards")]
    public required IReadOnlyList<FeatureCardResponse> Cards { get; init; }
}

public sealed class CompaniesContentResponse
{
    [JsonPropertyName("valueCards")]
    public required IReadOnlyList<FeatureCardResponse> ValueCards { get; init; }

    [JsonPropertyName("processCards")]
    public required IReadOnlyList<ProgressCardResponse> ProcessCards { get; init; }
}

public sealed class OpportunitiesContentResponse
{
    [JsonPropertyName("tracks")]
    public required IReadOnlyList<ProgressCardResponse> Tracks { get; init; }

    [JsonPropertyName("resources")]
    public required IReadOnlyList<ResourceCardResponse> Resources { get; init; }
}

public sealed class WomanTechContentResponse
{
    [JsonPropertyName("metrics")]
    public required IReadOnlyList<MetricItemResponse> Metrics { get; init; }

    [JsonPropertyName("journey")]
    public required IReadOnlyList<JourneyStepResponse> Journey { get; init; }
}

public sealed class JoinContentResponse
{
    [JsonPropertyName("metrics")]
    public required IReadOnlyList<MetricItemResponse> Metrics { get; init; }

    [JsonPropertyName("intakeOptions")]
    public required IReadOnlyList<SelectOptionResponse> IntakeOptions { get; init; }
}

public sealed class OrientaTechContentResponse
{
    [JsonPropertyName("metrics")]
    public required IReadOnlyList<MetricItemResponse> Metrics { get; init; }

    [JsonPropertyName("coreFeatures")]
    public required IReadOnlyList<FeatureCardResponse> CoreFeatures { get; init; }

    [JsonPropertyName("participationTracks")]
    public required IReadOnlyList<ProgressCardResponse> ParticipationTracks { get; init; }

    [JsonPropertyName("studySections")]
    public required IReadOnlyList<FeatureCardResponse> StudySections { get; init; }
}

public sealed class AboutContentResponse
{
    [JsonPropertyName("metrics")]
    public required IReadOnlyList<MetricItemResponse> Metrics { get; init; }

    [JsonPropertyName("socialLinks")]
    public required IReadOnlyList<SocialLinkResponse> SocialLinks { get; init; }

    [JsonPropertyName("teamZones")]
    public required IReadOnlyList<TeamZoneResponse> TeamZones { get; init; }
}

public sealed class TutorialsContentResponse
{
    [JsonPropertyName("featuredCategories")]
    public required IReadOnlyList<string> FeaturedCategories { get; init; }
}

public sealed class IntranetContentResponse
{
    [JsonPropertyName("ambassadorStatusOptions")]
    public required IReadOnlyList<SelectOptionResponse> AmbassadorStatusOptions { get; init; }

    [JsonPropertyName("ambassadorAvailabilityOptions")]
    public required IReadOnlyList<SelectOptionResponse> AmbassadorAvailabilityOptions { get; init; }

    [JsonPropertyName("staffPeriodOptions")]
    public required IReadOnlyList<SelectOptionResponse> StaffPeriodOptions { get; init; }

    [JsonPropertyName("memberCategoryOptions")]
    public required IReadOnlyList<string> MemberCategoryOptions { get; init; }

    [JsonPropertyName("sessionStatusOptions")]
    public required IReadOnlyList<string> SessionStatusOptions { get; init; }

    [JsonPropertyName("juniorSkillOptions")]
    public required IReadOnlyList<string> JuniorSkillOptions { get; init; }

    [JsonPropertyName("juniorAvailabilityOptions")]
    public required IReadOnlyList<SelectOptionResponse> JuniorAvailabilityOptions { get; init; }
}

public sealed class MetricItemResponse
{
    [JsonPropertyName("icon")]
    public required string Icon { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }

    [JsonPropertyName("label")]
    public required string Label { get; init; }
}

public sealed class FeatureCardResponse
{
    [JsonPropertyName("icon")]
    public required string Icon { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("points")]
    public required IReadOnlyList<string> Points { get; init; }
}

public sealed class ProgressCardResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("detail")]
    public required string Detail { get; init; }

    [JsonPropertyName("progress")]
    public required int Progress { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("ctaLabel")]
    public string? CtaLabel { get; init; }

    [JsonPropertyName("ctaLink")]
    public string? CtaLink { get; init; }
}

public sealed class ResourceCardResponse
{
    [JsonPropertyName("mode")]
    public required string Mode { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("summary")]
    public required string Summary { get; init; }

    [JsonPropertyName("tags")]
    public required IReadOnlyList<string> Tags { get; init; }

    [JsonPropertyName("meta")]
    public required string Meta { get; init; }

    [JsonPropertyName("ctaLabel")]
    public required string CtaLabel { get; init; }

    [JsonPropertyName("ctaLink")]
    public required string CtaLink { get; init; }
}

public sealed class HomeProfileCardResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("icon")]
    public required string Icon { get; init; }

    [JsonPropertyName("cta")]
    public required string Cta { get; init; }

    [JsonPropertyName("link")]
    public required string Link { get; init; }

    [JsonPropertyName("accent")]
    public required string Accent { get; init; }
}

public sealed class HomePastEventPhotoResponse
{
    [JsonPropertyName("src")]
    public required string Src { get; init; }

    [JsonPropertyName("alt")]
    public required string Alt { get; init; }

    [JsonPropertyName("label")]
    public required string Label { get; init; }
}

public sealed class ParticipationModeResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("detail")]
    public required string Detail { get; init; }
}

public sealed class GalleryGroupResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("subtitle")]
    public required string Subtitle { get; init; }

    [JsonPropertyName("items")]
    public required IReadOnlyList<GalleryItemResponse> Items { get; init; }
}

public sealed class GalleryItemResponse
{
    [JsonPropertyName("src")]
    public required string Src { get; init; }

    [JsonPropertyName("alt")]
    public required string Alt { get; init; }
}

public sealed class VideoCarouselItemResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("src")]
    public required string Src { get; init; }
}

public sealed class JourneyStepResponse
{
    [JsonPropertyName("step")]
    public required string Step { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }
}

public sealed class SelectOptionResponse
{
    [JsonPropertyName("label")]
    public required string Label { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}

public sealed class SocialLinkResponse
{
    [JsonPropertyName("platform")]
    public required string Platform { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }
}

public sealed class TeamMemberResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("role")]
    public required string Role { get; init; }

    [JsonPropertyName("photo")]
    public required string Photo { get; init; }

    [JsonPropertyName("photoAlt")]
    public required string PhotoAlt { get; init; }

    [JsonPropertyName("socials")]
    public required IReadOnlyList<SocialLinkResponse> Socials { get; init; }
}

public sealed class TeamZoneResponse
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("members")]
    public required IReadOnlyList<TeamMemberResponse> Members { get; init; }
}
