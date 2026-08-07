using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/tutorials")]
[Produces("application/json")]
public class TutorialesController : ControllerBase
{
    private readonly ITutorialesService _tutorialesService;
    private readonly ILogger<TutorialesController> _logger;

    public TutorialesController(ITutorialesService tutorialesService, ILogger<TutorialesController> logger)
    {
        _tutorialesService = tutorialesService ?? throw new ArgumentNullException(nameof(tutorialesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TutorialResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var tutorials = await _tutorialesService.GetAllTutorialsAsync(cancellationToken);
            return Ok(tutorials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tutorials");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TutorialResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var tutorial = await _tutorialesService.GetTutorialByIdAsync(id, cancellationToken);
            if (tutorial == null)
                return NotFound(new { error = "Tutorial not found" });

            return Ok(tutorial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tutorial {TutorialId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TutorialResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var tutorial = await _tutorialesService.GetTutorialBySlugAsync(slug, cancellationToken);
            if (tutorial == null)
                return NotFound(new { error = "Tutorial not found" });

            return Ok(tutorial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tutorial by slug {Slug}", slug);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("paginated")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { error = "Page number and size must be greater than 0" });

            var result = await _tutorialesService.GetPaginatedTutorialsAsync(pageNumber, pageSize, cancellationToken);
            return Ok(new { items = result.Items, totalCount = result.TotalCount, pageNumber, pageSize });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated tutorials");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TutorialResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateTutorialRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorial = await _tutorialesService.CreateTutorialAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = tutorial.Id }, tutorial);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation creating tutorial");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tutorial");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TutorialResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTutorialRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (id != request.Id)
                return BadRequest(new { error = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorial = await _tutorialesService.UpdateTutorialAsync(request, cancellationToken);
            return Ok(tutorial);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Tutorial not found: {TutorialId}", id);
            return NotFound(new { error = "Tutorial not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tutorial {TutorialId}", id);
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
            await _tutorialesService.DeleteTutorialAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Tutorial not found: {TutorialId}", id);
            return NotFound(new { error = "Tutorial not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tutorial {TutorialId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}
