using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Application.DTOs.Requests.FPTour;
using TechRiders.Application.DTOs.Responses.FPTour;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FPToursController : ControllerBase
{
    private readonly IFPTourService _tourService;

    public FPToursController(IFPTourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all FP tours")]
    [ProducesResponseType(typeof(IEnumerable<FPTourResponse>), 200)]
    public async Task<ActionResult<IEnumerable<FPTourResponse>>> GetAll(CancellationToken ct)
    {
        return Ok(await _tourService.GetAllFPToursAsync(ct));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get FP tour by ID")]
    [ProducesResponseType(typeof(FPTourResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<FPTourResponse>> GetById(Guid id, CancellationToken ct)
    {
        var tour = await _tourService.GetFPTourByIdAsync(id, ct);
        if (tour == null) return NotFound();
        return Ok(tour);
    }

    [HttpGet("center/{centerId:guid}")]
    [SwaggerOperation(Summary = "Get tours by center")]
    [ProducesResponseType(typeof(IEnumerable<FPTourResponse>), 200)]
    public async Task<ActionResult<IEnumerable<FPTourResponse>>> GetByCenter(Guid centerId, CancellationToken ct)
    {
        return Ok(await _tourService.GetFPToursByCenterAsync(centerId, ct));
    }

    [HttpGet("ambassador/{ambassadorId:guid}")]
    [SwaggerOperation(Summary = "Get tours by ambassador")]
    [ProducesResponseType(typeof(IEnumerable<FPTourResponse>), 200)]
    public async Task<ActionResult<IEnumerable<FPTourResponse>>> GetByAmbassador(Guid ambassadorId, CancellationToken ct)
    {
        return Ok(await _tourService.GetFPToursByAmbassadorAsync(ambassadorId, ct));
    }

    [HttpGet("pending")]
    [SwaggerOperation(Summary = "Get pending tours")]
    [ProducesResponseType(typeof(IEnumerable<FPTourResponse>), 200)]
    public async Task<ActionResult<IEnumerable<FPTourResponse>>> GetPending(CancellationToken ct)
    {
        return Ok(await _tourService.GetPendingFPToursAsync(ct));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create FP tour")]
    [ProducesResponseType(typeof(FPTourResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<FPTourResponse>> Create([FromBody] CreateFPTourRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var tour = await _tourService.CreateFPTourAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = tour.Id }, tour);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update FP tour")]
    [ProducesResponseType(typeof(FPTourResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<FPTourResponse>> Update(Guid id, [FromBody] UpdateFPTourRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var tour = await _tourService.UpdateFPTourAsync(id, request, ct);
            if (tour == null) return NotFound();
            return Ok(tour);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete FP tour")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _tourService.DeleteFPTourAsync(id, ct);
        if (!result) return NotFound();
        return NoContent();
    }
}
