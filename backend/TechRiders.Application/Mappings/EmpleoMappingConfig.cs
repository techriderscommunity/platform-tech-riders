
using Mapster;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Domain.Entities.Empleo;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Empleo domain (Job offers and applications)
/// </summary>
public class EmpleoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Oferta mappings
        config.NewConfig<Oferta, OfertaResponse>()
            .Map(dest => dest.Modalidad, src => (int)src.Modalidad)
            .Map(dest => dest.Estado, src => (int)src.Estado);

        // Candidatura mappings
        config.NewConfig<Candidatura, CandidaturaResponse>()
            .Map(dest => dest.Estado, src => (int)src.Estado);
    }
}
