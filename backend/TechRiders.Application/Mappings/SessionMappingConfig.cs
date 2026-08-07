using Mapster;
using TechRiders.Application.DTOs.Requests.Session;
using TechRiders.Application.DTOs.Responses.Sessions;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Session domain (Session entities and DTOs)
/// </summary>
public class SessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeo de entidad a response
        config.NewConfig<Session, SessionResponse>()
            .Map(dest => dest.Event, src => src.Event);
        // Mapeo de request a entidad
        config.NewConfig<CreateSessionRequest, Session>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.Event);

        // Mapeo de update request a entidad (solo actualiza propiedades no nulas)
        config.NewConfig<UpdateSessionRequest, Session>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.EventId)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.Event);
    }
}
