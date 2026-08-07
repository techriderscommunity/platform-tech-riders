using Mapster;
using TechRiders.Application.DTOs.Requests.Center;
using TechRiders.Application.DTOs.Responses.Center;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Center domain
/// </summary>
public class CenterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Center, CenterResponse>();
        config.NewConfig<CreateCenterRequest, Center>();
        config.NewConfig<UpdateCenterRequest, Center>()
            .Map(dest => dest, src => src, srcMember => srcMember != null);
    }
}
