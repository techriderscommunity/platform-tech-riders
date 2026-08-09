using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/candidaturas")]
[Route("api/applications")]
[Produces("application/json")]
public class ApplicationsController : ControllerBase
{
    private readonly IEmploymentService _empleoService;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IEmploymentService empleoService, ILogger<ApplicationsController> logger)
    {
        _empleoService = empleoService ?? throw new ArgumentNullException(nameof(empleoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("oferta/{ofertaId}")]
    [HttpGet("offer/{ofertaId}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CandidaturaResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByOferta(Guid ofertaId, CancellationToken cancellationToken)
    {
        try
        {
            var candidaturas = await _empleoService.GetCandidaturasByOfertaAsync(ofertaId, cancellationToken);
            return Ok(candidaturas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications for offer {OfertaId}", ofertaId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("junior/{juniorId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CandidaturaResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByJunior(string juniorId, CancellationToken cancellationToken)
    {
        try
        {
            var candidaturas = await _empleoService.GetCandidaturasByJuniorAsync(juniorId, cancellationToken);
            return Ok(candidaturas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications for junior {JuniorId}", juniorId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CandidaturaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var candidatura = await _empleoService.GetCandidaturaByIdAsync(id, cancellationToken);
            if (candidatura == null)
                return NotFound(new { error = "Application not found" });

            return Ok(candidatura);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application {CandidaturaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CandidaturaResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateCandidaturaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var candidatura = await _empleoService.CreateCandidaturaAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = candidatura.Id }, candidatura);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Application already exists");
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Job offer not found");
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost("{id}/advance-to-interview")]
    [HttpPost("{id}/advance")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CandidaturaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AdvanceToInterview(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var candidatura = await _empleoService.AdvanceToInterviewAsync(id, cancellationToken);
            return Ok(candidatura);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Application not found: {CandidaturaId}", id);
            return NotFound(new { error = "Application not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error advancing application {CandidaturaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CandidaturaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var candidatura = await _empleoService.RejectCandidaturaAsync(id, cancellationToken);
            return Ok(candidatura);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Application not found: {CandidaturaId}", id);
            return NotFound(new { error = "Application not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting application {CandidaturaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost("{id}/hire")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CandidaturaResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Hire(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var candidatura = await _empleoService.HireCandidaturaAsync(id, cancellationToken);
            return Ok(candidatura);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Application not found: {CandidaturaId}", id);
            return NotFound(new { error = "Application not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error hiring candidate {CandidaturaId}", id);
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
            await _empleoService.DeleteCandidaturaAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Application not found: {CandidaturaId}", id);
            return NotFound(new { error = "Application not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting application {CandidaturaId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}
