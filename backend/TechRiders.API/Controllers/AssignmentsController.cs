using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.Assignments;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Sugerencia de candidatos para asignar a iniciativas (sesiones, eventos, FPTour).</summary>
[ApiController]
[Route("api/assignments")]
[Produces("application/json")]
[Authorize(Policy = "permission:assignments.manage")]
public sealed class AssignmentsController : BaseApiController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet("candidates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentCandidateResponse>))]
    public async Task<IActionResult> GetCandidates([FromQuery] Guid? skillId, [FromQuery] Guid? availabilityValueId, CancellationToken cancellationToken)
    {
        var candidates = await _assignmentService.GetCandidatesAsync(skillId, availabilityValueId, cancellationToken);
        return Ok(candidates.Select(c => new AssignmentCandidateResponse
        {
            UserId = c.UserId,
            Name = c.Name,
            Email = c.Email,
            SkillLevel = c.SkillLevel,
            HasRequestedAvailability = c.HasRequestedAvailability,
            SessionsAsSpeaker = c.SessionsAsSpeaker,
            EventsParticipated = c.EventsParticipated,
            FPToursAsAmbassador = c.FPToursAsAmbassador,
        }));
    }
}
