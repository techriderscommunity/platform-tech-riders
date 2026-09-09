using Mapster;
using TechRiders.Application.DTOs.Requests.Center;
using TechRiders.Application.DTOs.Responses.Center;
using TechRiders.Application.Social;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Center domain
/// </summary>
public class CenterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Center, CenterResponse>()
            .Map(dest => dest.LinkedIn, src => SocialProfileUrls.Build(SocialProfileUrls.LinkedInSchool, src.LinkedIn))
            .Map(dest => dest.Instagram, src => SocialProfileUrls.Build(SocialProfileUrls.Instagram, src.Instagram))
            .Map(dest => dest.X, src => SocialProfileUrls.Build(SocialProfileUrls.X, src.X))
            .Map(dest => dest.YouTube, src => SocialProfileUrls.Build(SocialProfileUrls.YouTube, src.YouTube))
            .Map(dest => dest.Github, src => SocialProfileUrls.Build(SocialProfileUrls.GitHub, src.Github));
        config.NewConfig<CreateCenterRequest, Center>();
        config.NewConfig<UpdateCenterRequest, Center>()
            .Map(dest => dest, src => src, srcMember => srcMember != null);
    }
}
