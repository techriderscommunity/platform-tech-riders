using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Api.Contracts.Responses.Events;
using TechRiders.Application.DTOs.Requests.Event;
using TechRiders.Application.DTOs.Responses.Event;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>
/// Controlador para gestión de eventos
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventsController : BaseApiController
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(
        IEventService eventService,
        ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los eventos activos
    /// </summary>
    /// <returns>Lista de eventos</returns>
    /// <response code="200">Lista de eventos obtenida exitosamente</response>
    /// <response code="401">No autorizado - Token JWT requerido</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Obtener todos los eventos",
        Description = "Retorna una lista de todos los eventos activos en el sistema",
        OperationId = "GetAllEvents",
        Tags = new[] { "Events" }
    )]
    [SwaggerResponse(200, "Lista de eventos obtenida exitosamente", typeof(IEnumerable<EventResponse>))]
    [SwaggerResponse(401, "No autorizado - Token JWT requerido")]
    [SwaggerResponse(500, "Error interno del servidor")]
    [ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents(
        CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventService.GetAllEventsAsync(cancellationToken);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los eventos");
            return StatusCode(500, "Error al obtener los eventos");
        }
    }

    /// <summary>
    /// Obtiene un evento específico por su ID
    /// </summary>
    /// <param name="id">ID del evento</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Evento con sus sesiones</returns>
    /// <response code="200">Evento encontrado</response>
    /// <response code="404">Evento no encontrado</response>
    [HttpGet("{id:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Obtener evento por ID",
        Description = "Retorna un evento específico incluyendo sus sesiones asociadas",
        OperationId = "GetEventById"
    )]
    [SwaggerResponse(200, "Evento encontrado", typeof(EventResponse))]
    [SwaggerResponse(404, "Evento no encontrado")]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventResponse>> GetEventById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var evento = await _eventService.GetEventByIdAsync(id, cancellationToken);

            if (evento == null)
            {
                return NotFound($"Evento con ID {id} no encontrado");
            }

            return Ok(evento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener evento {EventoId}", id);
            return StatusCode(500, "Error al obtener el evento");
        }
    }

    /// <summary>
    /// Obtiene eventos próximos (futuros)
    /// </summary>
    /// <returns>Lista de eventos próximos</returns>
    [HttpGet("next")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Obtener eventos próximos",
        Description = "Retorna eventos cuya fecha de inicio es posterior a la fecha actual",
        OperationId = "GetUpcomingEvents"
    )]
    [SwaggerResponse(200, "Lista de eventos próximos", typeof(IEnumerable<EventResponse>))]
    [ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetUpcomingEvents(
        CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventService.GetUpcomingEventsAsync(cancellationToken);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener eventos próximos");
            return StatusCode(500, "Error al obtener los eventos próximos");
        }
    }

    /// <summary>
    /// Busca eventos por término de búsqueda
    /// </summary>
    /// <param name="searchTerm">Término a buscar en nombre o descripción</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de eventos que coinciden con la búsqueda</returns>
    [HttpGet("search")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Buscar eventos",
        Description = "Busca eventos por nombre o descripción",
        OperationId = "SearchEvents"
    )]
    [SwaggerResponse(200, "Resultados de búsqueda", typeof(IEnumerable<EventResponse>))]
    [ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventResponse>>> SearchEvents(
        [FromQuery] string searchTerm,
        CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventService.SearchEventsAsync(searchTerm, cancellationToken);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar eventos con término: {SearchTerm}", searchTerm);
            return StatusCode(500, "Error al buscar eventos");
        }
    }

    /// <summary>
    /// Obtiene eventos en un rango de fechas
    /// </summary>
    /// <param name="startDate">Fecha de inicio del rango</param>
    /// <param name="endDate">Fecha de fin del rango</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de eventos en el rango</returns>
    [HttpGet("by-dates")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Obtener eventos por rango de fechas",
        Description = "Retorna eventos que ocurren entre las fechas especificadas",
        OperationId = "GetEventsByDateRange"
    )]
    [SwaggerResponse(200, "Eventos en el rango de fechas", typeof(IEnumerable<EventResponse>))]
    [ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEventsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventService.GetEventsByDateRangeAsync(
                startDate, 
                endDate, 
                cancellationToken);

            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener eventos por rango de fechas");
            return StatusCode(500, "Error al obtener eventos por rango de fechas");
        }
    }

    [HttpGet("podcast/videos")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get podcast videos",
        Description = "Returns curated public podcast videos for the events page. Use playlist to filter specific collections.",
        OperationId = "GetPodcastVideos"
    )]
    [SwaggerResponse(200, "Podcast videos", typeof(IEnumerable<PodcastVideoResponse>))]
    [ProducesResponseType(typeof(IEnumerable<PodcastVideoResponse>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PodcastVideoResponse>> GetPodcastVideos(
        [FromQuery] int maxResults = 8,
        [FromQuery] string? playlist = null)
    {
        var safeLimit = Math.Clamp(maxResults, 1, 20);
        var normalizedPlaylist = (playlist ?? string.Empty).Trim().ToLowerInvariant();

        var videos = normalizedPlaylist switch
        {
            "profiles" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "J25VQJ7Wx34",
                    Title = "Perfiles profesionales · Episodio 1",
                    Url = "https://www.youtube.com/watch?v=J25VQJ7Wx34&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/J25VQJ7Wx34?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/J25VQJ7Wx34/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "X4mIfCx6XPU",
                    Title = "Perfiles profesionales · Episodio 2",
                    Url = "https://www.youtube.com/watch?v=X4mIfCx6XPU&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/X4mIfCx6XPU?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/X4mIfCx6XPU/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "vncHQDNPjEw",
                    Title = "Perfiles profesionales · Episodio 3",
                    Url = "https://www.youtube.com/watch?v=vncHQDNPjEw&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/vncHQDNPjEw?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/vncHQDNPjEw/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "A856m8nAx6g",
                    Title = "Perfiles profesionales · Episodio 4",
                    Url = "https://www.youtube.com/watch?v=A856m8nAx6g&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/A856m8nAx6g?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/A856m8nAx6g/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "5zfaHALRmis",
                    Title = "Perfiles profesionales · Episodio 5",
                    Url = "https://www.youtube.com/watch?v=5zfaHALRmis&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/5zfaHALRmis?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/5zfaHALRmis/hqdefault.jpg"
                }
            },
            "success-stories" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "HKgt8H8o-nI",
                    Title = "Historias de éxito · Episodio 1",
                    Url = "https://www.youtube.com/watch?v=HKgt8H8o-nI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/HKgt8H8o-nI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/HKgt8H8o-nI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "RXRqB_Ul_oI",
                    Title = "Historias de éxito · Episodio 2",
                    Url = "https://www.youtube.com/watch?v=RXRqB_Ul_oI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/RXRqB_Ul_oI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/RXRqB_Ul_oI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "zlZwB1VlY28",
                    Title = "Historias de éxito · Episodio 3",
                    Url = "https://www.youtube.com/watch?v=zlZwB1VlY28&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/zlZwB1VlY28?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/zlZwB1VlY28/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "TAxnDg0kyRI",
                    Title = "Historias de éxito · Episodio 4",
                    Url = "https://www.youtube.com/watch?v=TAxnDg0kyRI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/TAxnDg0kyRI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/TAxnDg0kyRI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "NwEhryRqSio",
                    Title = "Historias de éxito · Episodio 5",
                    Url = "https://www.youtube.com/watch?v=NwEhryRqSio&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/NwEhryRqSio?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/NwEhryRqSio/hqdefault.jpg"
                }
            },
            "interviews" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "WQp9pZb8shU",
                    Title = "Entrevistas · IA, Copilot y el futuro del desarrollo",
                    Url = "https://www.youtube.com/watch?v=WQp9pZb8shU&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/WQp9pZb8shU?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/WQp9pZb8shU/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "SvZ50wArtaM",
                    Title = "Entrevistas · Agentes de IA en empresa",
                    Url = "https://www.youtube.com/watch?v=SvZ50wArtaM&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/SvZ50wArtaM?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/SvZ50wArtaM/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "baKNCZUbvL8",
                    Title = "Entrevistas · Estudiantes AcademyVerso",
                    Url = "https://www.youtube.com/watch?v=baKNCZUbvL8&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/baKNCZUbvL8?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/baKNCZUbvL8/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "-biqjBJN_cI",
                    Title = "Entrevistas · IA con imágenes",
                    Url = "https://www.youtube.com/watch?v=-biqjBJN_cI&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/-biqjBJN_cI?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/-biqjBJN_cI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "Gc2sLw3vcvM",
                    Title = "Entrevistas · Microsoft Student Ambassador",
                    Url = "https://www.youtube.com/watch?v=Gc2sLw3vcvM&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/Gc2sLw3vcvM?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/Gc2sLw3vcvM/hqdefault.jpg"
                }
            },
            _ => new[]
        {
            new PodcastVideoResponse
            {
                VideoId = "YekC-fVM3Ig",
                Title = "Comunidad, aprendizaje y cerrar ciclos: Tech Riders Talks | Salero de Ming",
                Url = "https://www.youtube.com/watch?v=YekC-fVM3Ig",
                EmbedUrl = "https://www.youtube-nocookie.com/embed/YekC-fVM3Ig",
                ThumbnailUrl = "https://i.ytimg.com/vi/YekC-fVM3Ig/hqdefault.jpg"
            },
            new PodcastVideoResponse
            {
                VideoId = "NHkw3rh1BO8",
                Title = "Liderazgo técnico, comunidad y crecimiento profesional | Sergio Hernández",
                Url = "https://www.youtube.com/watch?v=NHkw3rh1BO8",
                EmbedUrl = "https://www.youtube-nocookie.com/embed/NHkw3rh1BO8",
                ThumbnailUrl = "https://i.ytimg.com/vi/NHkw3rh1BO8/hqdefault.jpg"
            },
            new PodcastVideoResponse
            {
                VideoId = "qJUUlvvH3_g",
                Title = "IA, liderazgo y comunidad: experiencia sin filtros | Javier Pallo",
                Url = "https://www.youtube.com/watch?v=qJUUlvvH3_g",
                EmbedUrl = "https://www.youtube-nocookie.com/embed/qJUUlvvH3_g",
                ThumbnailUrl = "https://i.ytimg.com/vi/qJUUlvvH3_g/hqdefault.jpg"
            },
            new PodcastVideoResponse
            {
                VideoId = "IOi91LjE0m4",
                Title = "Ciberseguridad real: pentesting, red team y LockShields | Marco Carrasco",
                Url = "https://www.youtube.com/watch?v=IOi91LjE0m4",
                EmbedUrl = "https://www.youtube-nocookie.com/embed/IOi91LjE0m4",
                ThumbnailUrl = "https://i.ytimg.com/vi/IOi91LjE0m4/hqdefault.jpg"
            },
            new PodcastVideoResponse
            {
                VideoId = "o6bGKi8y2eY",
                Title = "De junior a senior: claves reales para crecer en tecnología | María & Elías",
                Url = "https://www.youtube.com/watch?v=o6bGKi8y2eY",
                EmbedUrl = "https://www.youtube-nocookie.com/embed/o6bGKi8y2eY",
                ThumbnailUrl = "https://i.ytimg.com/vi/o6bGKi8y2eY/hqdefault.jpg"
            },
        }
        };

        return Ok(videos.Take(safeLimit));
    }

    /// <summary>
    /// Crea un nuevo evento
    /// </summary>
    /// <param name="request">Datos del evento a crear</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Evento creado</returns>
    /// <response code="201">Evento creado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear nuevo evento",
        Description = "Crea un nuevo evento en el sistema. La fecha de finalización debe ser posterior a la fecha de inicio.",
        OperationId = "CreateEvent"
    )]
    [SwaggerResponse(201, "Evento creado exitosamente", typeof(EventResponse))]
    [SwaggerResponse(400, "Datos inválidos o fechas incorrectas")]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventResponse>> CreateEvent(
        [FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var evento = await _eventService.CreateEventAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetEventById),
                new { id = evento.Id },
                evento);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio fallida al crear evento");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear evento");
            return StatusCode(500, "Error al crear el evento");
        }
    }

    /// <summary>
    /// Actualiza un evento existente
    /// </summary>
    /// <param name="id">ID del evento a actualizar</param>
    /// <param name="request">Datos a actualizar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Evento actualizado</returns>
    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Actualizar evento",
        Description = "Actualiza los datos de un evento existente. Solo se actualizan los campos proporcionados.",
        OperationId = "UpdateEvent"
    )]
    [SwaggerResponse(200, "Evento actualizado exitosamente", typeof(EventResponse))]
    [SwaggerResponse(400, "Datos inválidos")]
    [SwaggerResponse(404, "Evento no encontrado")]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventResponse>> UpdateEvent(
        [FromRoute] Guid id,
        [FromBody] UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var evento = await _eventService.UpdateEventAsync(id, request, cancellationToken);

            if (evento == null)
            {
                return NotFound($"Evento con ID {id} no encontrado");
            }

            return Ok(evento);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio fallida al actualizar evento {EventoId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar evento {EventoId}", id);
            return StatusCode(500, "Error al actualizar el evento");
        }
    }

    /// <summary>
    /// Elimina lógicamente un evento
    /// </summary>
    /// <param name="id">ID del evento a eliminar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de eliminación</returns>
    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(
        Summary = "Eliminar evento",
        Description = "Realiza una eliminación lógica del evento (soft delete). El evento no se borra físicamente de la base de datos.",
        OperationId = "DeleteEvento"
    )]
    [SwaggerResponse(204, "Evento eliminado exitosamente")]
    [SwaggerResponse(404, "Evento no encontrado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _eventService.DeleteEventAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Evento con ID {id} no encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar evento {EventoId}", id);
            return StatusCode(500, "Error al eliminar el evento");
        }
    }
}
