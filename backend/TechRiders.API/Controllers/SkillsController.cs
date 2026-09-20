using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Skills;
using TechRiders.Api.Contracts.Responses.Skills;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Cat\u00e1logo de skills y gesti\u00f3n de las skills del propio perfil (Mi Perfil).</summary>
[ApiController]
[Route("api/skills")]
[Produces("application/json")]
[Authorize]
public sealed class SkillsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public SkillsController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SkillResponse>))]
    public async Task<IActionResult> GetCatalog(CancellationToken cancellationToken)
    {
        var skills = await SkillsService.GetCatalogAsync(_dbContext, cancellationToken);
        return Ok(skills.Select(s => new SkillResponse { Id = s.Id, Name = s.Name, Description = s.Description, ParentSkillId = s.ParentSkillId }));
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserSkillResponse>))]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var skills = await SkillsService.GetUserSkillsAsync(_dbContext, userId.Value, cancellationToken);
        return Ok(skills.Select(ToResponse));
    }

    [HttpPost("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSkillResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrUpdateMine([FromBody] AddUserSkillRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var userSkill = await SkillsService.AddOrUpdateAsync(_dbContext, userId.Value, request.SkillId, request.Level, request.IsSpeakerSkill, request.IsMentorSkill, cancellationToken);
            return Ok(new UserSkillResponse
            {
                SkillId = userSkill.SkillId,
                SkillName = userSkill.Skill.Name,
                Level = userSkill.Level.ToString(),
                IsSpeakerSkill = userSkill.IsSpeakerSkill,
                IsMentorSkill = userSkill.IsMentorSkill,
            });
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpDelete("me/{skillId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveMine(Guid skillId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            await SkillsService.RemoveAsync(_dbContext, userId.Value, skillId, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    private static UserSkillResponse ToResponse(Domain.Entities.UserSkill userSkill) => new()
    {
        SkillId = userSkill.SkillId,
        SkillName = userSkill.Skill.Name,
        Level = userSkill.Level.ToString(),
        IsSpeakerSkill = userSkill.IsSpeakerSkill,
        IsMentorSkill = userSkill.IsMentorSkill,
    };
}
