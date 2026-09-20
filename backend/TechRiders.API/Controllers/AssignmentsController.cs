using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.Assignments;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Sugerencia de candidatos para asignar a iniciativas (sesiones, eventos, FPTour).</summary>
[ApiController]
[Route("api/assignments")]
[Produces("application/json")]
[Authorize(Policy = "permission:assignments.manage")]
public sealed class AssignmentsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public AssignmentsController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("candidates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentCandidateResponse>))]
    public async Task<IActionResult> GetCandidates([FromQuery] Guid? skillId, [FromQuery] Guid? availabilityValueId, CancellationToken cancellationToken)
    {
        var candidates = await AssignmentSuggestionService.GetCandidatesAsync(_dbContext, skillId, availabilityValueId, cancellationToken);
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
