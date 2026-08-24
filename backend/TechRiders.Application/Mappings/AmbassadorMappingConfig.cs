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
        config.NewConfig<User, AmbassadorResponse>()
            .Map(dest => dest.CategoryId, src => (int?)null)
            .Map(dest => dest.CategoryName, src => (string?)null)
            .Map(dest => dest.OtherCategory, src => (string?)null)
            .Map(dest => dest.Skill, src => (string?)null);

        // Request to Entity
        config.NewConfig<CreateAmbassadorRequest, User>()
            .Map(dest => dest.Nickname, src => string.IsNullOrWhiteSpace(src.Nickname) ? src.Email.Split('@')[0] : src.Nickname)
            .Map(dest => dest.IsActive, src => true);

        config.NewConfig<UpdateAmbassadorRequest, User>()
            .Map(dest => dest, src => src, src => src != null);
    }
}
