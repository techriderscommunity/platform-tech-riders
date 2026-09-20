using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Users;
using TechRiders.Api.Contracts.Responses.Users;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;

namespace TechRiders.Api.Controllers;

/// <summary>Gesti\u00f3n completa de usuarios de la comunidad para el panel de Staff/Admin.</summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Authorize(Policy = "permission:users.manage")]
public sealed class UsersController : BaseApiController
{
    private readonly IUserAdminService _userAdminService;

    public UsersController(IUserAdminService userAdminService)
    {
        _userAdminService = userAdminService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserListResponse))]
    public async Task<IActionResult> List(
        [FromQuery] string? search, [FromQuery] string? role, [FromQuery] string? membershipStatus,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _userAdminService.ListAsync(search, role, membershipStatus, page, pageSize, cancellationToken);

        return Ok(new UserListResponse
        {
            Items = items.Select(ToListItem).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetailResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userAdminService.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(ToDetail(user));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var user = await _userAdminService.CreateAsync(request.Nickname, request.Name, request.LastName, request.Email, request.Phone, request.Locality, request.About, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { user.Id });
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var user = await _userAdminService.UpdateAsync(id, request.Name, request.LastName, request.Email, request.Phone, request.Locality, request.About, cancellationToken);
            return Ok(new { user.Id });
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
        }
    }

    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _userAdminService.ActivateAsync(id, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
        }
    }

    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _userAdminService.DeactivateAsync(id, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
        }
    }

    [HttpPost("{id:guid}/roles/{roleName}/revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeRole(Guid id, string roleName, CancellationToken cancellationToken)
    {
        try
        {
            await _userAdminService.RevokeRoleAsync(id, roleName, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
        }
    }

    [HttpGet("{id:guid}/activity")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserActivityResponse))]
    public async Task<IActionResult> GetActivity(Guid id, CancellationToken cancellationToken)
    {
        var summary = await _userAdminService.GetActivityAsync(id, cancellationToken);
        return Ok(new UserActivityResponse
        {
            EventsRegistered = summary.EventsRegistered,
            SessionsRegistered = summary.SessionsRegistered,
            SpeakerSessions = summary.SpeakerSessions,
            FPToursAsAmbassador = summary.FPToursAsAmbassador,
            RecentActions = summary.RecentActions.Select(a => new UserAuditActionResponse
            {
                CreatedUtc = a.CreatedUtc,
                Module = a.Module,
                Action = a.Action,
                Result = a.Result,
                Detail = a.Detail,
            }).ToList(),
        });
    }

    private static UserListItemResponse ToListItem(User user) => new()
    {
        Id = user.Id,
        Nickname = user.Nickname,
        Name = user.Name,
        LastName = user.LastName,
        Email = user.Email,
        Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
        MembershipStatus = user.Membership?.Status.ToString(),
        IsWorking = user.IsWorking,
        LastActivityDate = user.LastActivityDate,
    };

    private static UserDetailResponse ToDetail(User user) => new()
    {
        Id = user.Id,
        Nickname = user.Nickname,
        Name = user.Name,
        LastName = user.LastName,
        Email = user.Email,
        Phone = user.Phone,
        Locality = user.Locality,
        About = user.About,
        LinkedIn = user.LinkedIn,
        Instagram = user.Instagram,
        X = user.X,
        YouTube = user.YouTube,
        Github = user.Github,
        Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
        MembershipStatus = user.Membership?.Status.ToString(),
        CurrentProfile = user.ProfileHistories.FirstOrDefault(p => p.IsCurrent)?.Profile.Name,
        ActiveCapabilities = user.Capabilities.Where(c => c.Status == Domain.Enums.CapabilityStatus.Activa).Select(c => c.Capability.Name).ToList(),
        Organizations = user.OrganizationRelations.Select(po => new UserOrganizationSummary
        {
            OrganizationId = po.OrganizationId,
            OrganizationName = po.Organization?.Name ?? string.Empty,
            RelationType = po.RelationType.ToString(),
            Status = po.Status.ToString(),
        }).ToList(),
        Skills = user.UserSkills.Select(us => new UserSkillSummary
        {
            SkillId = us.SkillId,
            SkillName = us.Skill.Name,
            Level = us.Level.ToString(),
        }).ToList(),
        CreatedAt = user.CreatedAt,
    };
}
