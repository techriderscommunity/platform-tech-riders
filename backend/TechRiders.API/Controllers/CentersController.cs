using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Application.DTOs.Requests.Center;
using TechRiders.Application.DTOs.Responses.Center;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CentersController : BaseApiController
{
    private readonly ICenterService _centerService;

    public CentersController(ICenterService centerService)
    {
        _centerService = centerService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all centers")]
    [ProducesResponseType(typeof(IEnumerable<CenterResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CenterResponse>>> GetAll(CancellationToken ct)
    {
        return Ok(await _centerService.GetAllCentersAsync(ct));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get center by ID")]
    [ProducesResponseType(typeof(CenterResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CenterResponse>> GetById(Guid id, CancellationToken ct)
    {
        var center = await _centerService.GetCenterByIdAsync(id, ct);
        if (center == null) return NotFound();
        return Ok(center);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search centers")]
    [ProducesResponseType(typeof(IEnumerable<CenterResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CenterResponse>>> Search([FromQuery] string searchTerm, CancellationToken ct)
    {
        return Ok(await _centerService.SearchCentersAsync(searchTerm, ct));
    }

    [HttpGet("locality/{locality}")]
    [SwaggerOperation(Summary = "Get centers by locality")]
    [ProducesResponseType(typeof(IEnumerable<CenterResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CenterResponse>>> GetByLocality(string locality, CancellationToken ct)
    {
        return Ok(await _centerService.GetCentersByLocalityAsync(locality, ct));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create center")]
    [ProducesResponseType(typeof(CenterResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CenterResponse>> Create([FromBody] CreateCenterRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var center = await _centerService.CreateCenterAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = center.Id }, center);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update center")]
    [ProducesResponseType(typeof(CenterResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CenterResponse>> Update(Guid id, [FromBody] UpdateCenterRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var center = await _centerService.UpdateCenterAsync(id, request, ct);
        if (center == null) return NotFound();
        return Ok(center);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete center")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _centerService.DeleteCenterAsync(id, ct);
        if (!result) return NotFound();
        return NoContent();
    }
}
