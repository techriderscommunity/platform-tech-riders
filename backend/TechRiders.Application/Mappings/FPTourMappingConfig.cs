using Mapster;
using TechRiders.Application.DTOs.Requests.FPTour;
using TechRiders.Application.DTOs.Responses.FPTour;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for FPTour domain (FPTour entities and DTOs)
/// </summary>
public class FPTourMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FPTour, FPTourResponse>()
            .Map(dest => dest.CenterName, src => src.Center.Name)
            .Map(dest => dest.AmbassadorName, src => $"{src.Ambassador.Name} {src.Ambassador.LastName}");

        config.NewConfig<CreateFPTourRequest, FPTour>();

        config.NewConfig<UpdateFPTourRequest, FPTour>()
            .Map(dest => dest, src => src, srcMember => srcMember != null);
    }
}
