using Mapster;
using TechRiders.Application.DTOs.Requests.Event;
using TechRiders.Application.DTOs.Responses.Event;
using TechRiders.Application.DTOs.Responses.Sessions;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Event domain
/// </summary>
public class EventMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeo de entidad a response
        config.NewConfig<Event, EventResponse>()
            .Map(dest => dest.Sessions, src => src.Sessions);

        config.NewConfig<Event, EventBasicResponse>();

        // Mapeo de request a entidad
        config.NewConfig<CreateEventRequest, Event>();

        // Mapeo de update request a entidad (solo actualiza propiedades no nulas)
        config.NewConfig<UpdateEventRequest, Event>()
            .Map(dest => dest, src => src, srcMember => srcMember != null);
    }
}
