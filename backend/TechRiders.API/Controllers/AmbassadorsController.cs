using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Application.DTOs.Requests.Ambassador;
using TechRiders.Application.DTOs.Responses.Ambassador;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AmbassadorsController : BaseApiController
{
    private readonly IAmbassadorService _ambassadorService;
    private readonly ILogger<AmbassadorsController> _logger;

    public AmbassadorsController(IAmbassadorService ambassadorService, ILogger<AmbassadorsController> logger)
    {
        _ambassadorService = ambassadorService;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all active ambassador users", Description = "Returns active users that currently hold the ambassador role")]
    [SwaggerResponse(200, "List of ambassadors obtained successfully", typeof(IEnumerable<AmbassadorResponse>))]
    [ProducesResponseType(typeof(IEnumerable<AmbassadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AmbassadorResponse>>> GetAllAmbassadors(CancellationToken cancellationToken)
    {
        var ambassadors = await _ambassadorService.GetAllAmbassadorsAsync(cancellationToken);
        return Ok(ambassadors);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get ambassador user by ID", Description = "Returns an active user with ambassador role by user ID")]
    [SwaggerResponse(200, "Ambassador found", typeof(AmbassadorResponse))]
    [SwaggerResponse(404, "Ambassador not found")]
    [ProducesResponseType(typeof(AmbassadorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AmbassadorResponse>> GetAmbassadorById(Guid id, CancellationToken cancellationToken)
    {
        var ambassador = await _ambassadorService.GetAmbassadorByIdAsync(id, cancellationToken);
        if (ambassador == null) return NotFound(new { message = $"Ambassador with ID {id} not found" });
        return Ok(ambassador);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search ambassador users", Description = "Search active users with ambassador role by nickname, name, last name or email")]
    [SwaggerResponse(200, "Search results", typeof(IEnumerable<AmbassadorResponse>))]
    [ProducesResponseType(typeof(IEnumerable<AmbassadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AmbassadorResponse>>> SearchAmbassadors(
        [FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        var ambassadors = await _ambassadorService.SearchAmbassadorsAsync(searchTerm, cancellationToken);
        return Ok(ambassadors);
    }

    [HttpGet("category/{categoryId:int}")]
    [SwaggerOperation(Summary = "Get ambassadors by category")]
    [SwaggerResponse(200, "Ambassadors in category", typeof(IEnumerable<AmbassadorResponse>))]
    [ProducesResponseType(typeof(IEnumerable<AmbassadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AmbassadorResponse>>> GetByCategory(
        int categoryId, CancellationToken cancellationToken)
    {
        var ambassadors = await _ambassadorService.GetAmbassadorsByCategoryAsync(categoryId, cancellationToken);
        return Ok(ambassadors);
    }

    [HttpGet("working")]
    [SwaggerOperation(Summary = "Get working ambassadors")]
    [SwaggerResponse(200, "Working ambassadors", typeof(IEnumerable<AmbassadorResponse>))]
    [ProducesResponseType(typeof(IEnumerable<AmbassadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AmbassadorResponse>>> GetWorking(CancellationToken cancellationToken)
    {
        var ambassadors = await _ambassadorService.GetWorkingAmbassadorsAsync(cancellationToken);
        return Ok(ambassadors);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create ambassador user profile")]
    [SwaggerResponse(201, "Ambassador created successfully", typeof(AmbassadorResponse))]
    [SwaggerResponse(400, "Invalid request")]
    [ProducesResponseType(typeof(AmbassadorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AmbassadorResponse>> CreateAmbassador(
        [FromBody] CreateAmbassadorRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var ambassador = await _ambassadorService.CreateAmbassadorAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAmbassadorById), new { id = ambassador.Id }, ambassador);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update ambassador user profile")]
    [SwaggerResponse(200, "Ambassador updated successfully", typeof(AmbassadorResponse))]
    [SwaggerResponse(404, "Ambassador not found")]
    [SwaggerResponse(400, "Invalid request")]
    [ProducesResponseType(typeof(AmbassadorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AmbassadorResponse>> UpdateAmbassador(
        Guid id, [FromBody] UpdateAmbassadorRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var ambassador = await _ambassadorService.UpdateAmbassadorAsync(id, request, cancellationToken);
            if (ambassador == null) return NotFound(new { message = $"Ambassador with ID {id} not found" });
            return Ok(ambassador);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Remove ambassador role from user")]
    [SwaggerResponse(204, "Ambassador deleted successfully")]
    [SwaggerResponse(404, "Ambassador not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAmbassador(Guid id, CancellationToken cancellationToken)
    {
        var result = await _ambassadorService.DeleteAmbassadorAsync(id, cancellationToken);
        if (!result) return NotFound(new { message = $"Ambassador with ID {id} not found" });
        return NoContent();
    }
}
