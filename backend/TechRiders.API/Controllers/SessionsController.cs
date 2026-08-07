using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Application.DTOs.Requests.Session;
using TechRiders.Application.DTOs.Responses.Sessions;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>
/// Controlador para gestión de sesiones
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SessionsController : BaseApiController
{
    private readonly ISessionService _sessionService;
    private readonly ILogger<SessionsController> _logger;

    public SessionsController(
        ISessionService sessionService,
        ILogger<SessionsController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las sesiones activas
    /// </summary>
    /// <returns>Lista de sesiones</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener todas las sesiones",
        Description = "Retorna una lista de todas las sesiones activas en el sistema",
        OperationId = "GetAllSesiones"
    )]
    [SwaggerResponse(200, "Lista de sesiones obtenida exitosamente", typeof(IEnumerable<SessionResponse>))]
    [ProducesResponseType(typeof(IEnumerable<SessionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetAllSesiones(
        CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _sessionService.GetAllSessionsAsync(cancellationToken);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las sesiones");
            return StatusCode(500, "Error al obtener las sesiones");
        }
    }

    /// <summary>
    /// Obtiene una sesión específica por su ID
    /// </summary>
    /// <param name="id">ID de la sesión</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Sesión con información del evento</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Obtener sesión por ID",
        Description = "Retorna una sesión específica incluyendo información del evento asociado",
        OperationId = "GetSessionById"
    )]
    [SwaggerResponse(200, "Sesión encontrada", typeof(SessionResponse))]
    [SwaggerResponse(404, "Sesión no encontrada")]
    [ProducesResponseType(typeof(SessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionResponse>> GetSessionById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var session = await _sessionService.GetSessionByIdAsync(id, cancellationToken);

            if (session == null)
            {
                return NotFound($"Sesión con ID {id} no encontrada");
            }

            return Ok(session);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesión {SessionId}", id);
            return StatusCode(500, "Error al obtener la sesión");
        }
    }

    /// <summary>
    /// Obtiene todas las sesiones de un evento específico
    /// </summary>
    /// <param name="eventId">ID del evento</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de sesiones del evento</returns>
    [HttpGet("evento/{eventoId:guid}")]
    [HttpGet("event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Obtener sesiones por evento",
        Description = "Retorna todas las sesiones asociadas a un evento específico",
        OperationId = "GetSesionesByEvento"
    )]
    [SwaggerResponse(200, "Lista de sesiones del evento", typeof(IEnumerable<SessionResponse>))]
    [ProducesResponseType(typeof(IEnumerable<SessionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionsByEvent(
        [FromRoute] Guid eventId,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _sessionService.GetSessionsByEventIdAsync(eventId, cancellationToken);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesiones del evento {EventId}", eventId);
            return StatusCode(500, "Error al obtener las sesiones del evento");
        }
    }

    /// <summary>
    /// Obtiene sesiones por nombre de ponente
    /// </summary>
    /// <param name="speaker">Nombre del ponente</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de sesiones del ponente</returns>
    [HttpGet("ponente/{speaker}")]
    [HttpGet("speaker/{speaker}")]
    [SwaggerOperation(
        Summary = "Obtener sesiones por ponente",
        Description = "Retorna todas las sesiones impartidas por un ponente específico",
        OperationId = "GetSessionsBySpeaker"
    )]
    [SwaggerResponse(200, "Lista de sesiones del ponente", typeof(IEnumerable<SessionResponse>))]
    [ProducesResponseType(typeof(IEnumerable<SessionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionsBySpeaker(
        [FromRoute] string speaker,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _sessionService.GetSessionsBySpeakerAsync(speaker, cancellationToken);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesiones del ponente {Speaker}", speaker);
            return StatusCode(500, "Error al obtener las sesiones del ponente");
        }
    }

    /// <summary>
    /// Obtiene sesiones por nivel de dificultad
    /// </summary>
    /// <param name="level">Nivel de dificultad (Básico, Intermedio, Avanzado)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de sesiones del nivel especificado</returns>
    [HttpGet("nivel/{level}")]
    [HttpGet("level/{level}")]
    [SwaggerOperation(
        Summary = "Obtener sesiones por nivel",
        Description = "Retorna todas las sesiones de un nivel de dificultad específico",
        OperationId = "GetSessionsByLevel"
    )]
    [SwaggerResponse(200, "Lista de sesiones del nivel", typeof(IEnumerable<SessionResponse>))]
    [ProducesResponseType(typeof(IEnumerable<SessionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionsByLevel(
        [FromRoute] string level,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _sessionService.GetSessionsByLevelAsync(level, cancellationToken);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sesiones del nivel {Level}", level);
            return StatusCode(500, "Error al obtener las sesiones del nivel");
        }
    }

    /// <summary>
    /// Crea una nueva sesión
    /// </summary>
    /// <param name="request">Datos de la sesión a crear</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Sesión creada</returns>
    /// <response code="201">Sesión creada exitosamente</response>
    /// <response code="400">Datos inválidos, horarios incorrectos o conflicto en la sala</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear nueva sesión",
        Description = "Crea una nueva sesión. Valida que la hora de fin sea posterior a la hora de inicio y que no haya conflictos de horario en la sala.",
        OperationId = "CreateSession"
    )]
    [SwaggerResponse(201, "Sesión creada exitosamente", typeof(SessionResponse))]
    [SwaggerResponse(400, "Datos inválidos, horarios incorrectos o conflicto en la sala")]
    [ProducesResponseType(typeof(SessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionResponse>> CreateSession(
        [FromBody] CreateSessionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = await _sessionService.CreateSessionAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetSessionById),
                new { id = session.Id },
                session);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio fallida al crear sesión");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear sesión");
            return StatusCode(500, "Error al crear la sesión");
        }
    }

    /// <summary>
    /// Actualiza una sesión existente
    /// </summary>
    /// <param name="id">ID de la sesión a actualizar</param>
    /// <param name="request">Datos a actualizar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Sesión actualizada</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Actualizar sesión",
        Description = "Actualiza los datos de una sesión existente. Solo se actualizan los campos proporcionados.",
        OperationId = "UpdateSession"
    )]
    [SwaggerResponse(200, "Sesión actualizada exitosamente", typeof(SessionResponse))]
    [SwaggerResponse(400, "Datos inválidos o conflicto de horarios")]
    [SwaggerResponse(404, "Sesión no encontrada")]
    [ProducesResponseType(typeof(SessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionResponse>> UpdateSession(
        [FromRoute] Guid id,
        [FromBody] UpdateSessionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = await _sessionService.UpdateSessionAsync(id, request, cancellationToken);

            if (session == null)
            {
                return NotFound($"Sesión con ID {id} no encontrada");
            }

            return Ok(session);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio fallida al actualizar sesión {SessionId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar sesión {SessionId}", id);
            return StatusCode(500, "Error al actualizar la sesión");
        }
    }

    /// <summary>
    /// Elimina lógicamente una sesión
    /// </summary>
    /// <param name="id">ID de la sesión a eliminar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de eliminación</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Eliminar sesión",
        Description = "Realiza una eliminación lógica de la sesión (soft delete). La sesión no se borra físicamente de la base de datos.",
        OperationId = "DeleteSession"
    )]
    [SwaggerResponse(204, "Sesión eliminada exitosamente")]
    [SwaggerResponse(404, "Sesión no encontrada")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSession(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _sessionService.DeleteSessionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Sesión con ID {id} no encontrada");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar sesión {SessionId}", id);
            return StatusCode(500, "Error al eliminar la sesión");
        }
    }
}
