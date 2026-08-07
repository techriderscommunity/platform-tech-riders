using Microsoft.AspNetCore.Mvc;

namespace TechRiders.Api.Controllers;

/// <summary>
/// Controlador base con configuración común para todos los controladores
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Crea una respuesta de error estandarizada
    /// </summary>
    protected IActionResult CreateErrorResponse(string message, int statusCode = 400)
    {
        return StatusCode(statusCode, new
        {
            Success = false,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Crea una respuesta de éxito estandarizada
    /// </summary>
    protected IActionResult CreateSuccessResponse<T>(T data, string? message = null)
    {
        return Ok(new
        {
            Success = true,
            Message = message,
            Data = data,
            Timestamp = DateTime.UtcNow
        });
    }
}
