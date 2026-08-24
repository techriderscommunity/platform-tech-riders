using Mapster;
using TechRiders.Application.DTOs.Responses.Knowledge;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Tutoriales domain (Tutoriales entities and DTOs)
/// </summary>
public class TutorialesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tutorial, TutorialResponse>();
    }
}
