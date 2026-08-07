using Mapster;
using TechRiders.Application.DTOs.Requests.Ambassador;
using TechRiders.Application.DTOs.Responses.Ambassador;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Ambassador domain
/// </summary>
public class AmbassadorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity to Response
        config.NewConfig<Ambassador, AmbassadorResponse>()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null);

        // Request to Entity
        config.NewConfig<CreateAmbassadorRequest, Ambassador>();

        config.NewConfig<UpdateAmbassadorRequest, Ambassador>()
            .Map(dest => dest, src => src, src => src != null);
    }
}
