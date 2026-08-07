using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/offers")]
[Produces("application/json")]
public class OfertasController : ControllerBase
{
    private readonly IEmpleoService _empleoService;
    private readonly ILogger<OfertasController> _logger;

    public OfertasController(IEmpleoService empleoService, ILogger<OfertasController> logger)
    {
        _empleoService = empleoService ?? throw new ArgumentNullException(nameof(empleoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OfertaResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var ofertas = await _empleoService.GetAllOfertasAsync(cancellationToken);
            return Ok(ofertas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job offers");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfertaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var oferta = await _empleoService.GetOfertaByIdAsync(id, cancellationToken);
            if (oferta == null)
                return NotFound(new { error = "Job offer not found" });

            return Ok(oferta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job offer {OfertaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OfertaResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateOfertaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var oferta = await _empleoService.CreateOfertaAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = oferta.Id }, oferta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job offer");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfertaResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfertaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (id != request.Id)
                return BadRequest(new { error = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var oferta = await _empleoService.UpdateOfertaAsync(request, cancellationToken);
            return Ok(oferta);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Job offer not found: {OfertaId}", id);
            return NotFound(new { error = "Job offer not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job offer {OfertaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost("{id}/publish")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfertaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var oferta = await _empleoService.PublishOfertaAsync(id, cancellationToken);
            return Ok(oferta);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Job offer not found: {OfertaId}", id);
            return NotFound(new { error = "Job offer not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing job offer {OfertaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost("{id}/close")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfertaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var oferta = await _empleoService.CloseOfertaAsync(id, cancellationToken);
            return Ok(oferta);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Job offer not found: {OfertaId}", id);
            return NotFound(new { error = "Job offer not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing job offer {OfertaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _empleoService.DeleteOfertaAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Job offer not found: {OfertaId}", id);
            return NotFound(new { error = "Job offer not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job offer {OfertaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}
