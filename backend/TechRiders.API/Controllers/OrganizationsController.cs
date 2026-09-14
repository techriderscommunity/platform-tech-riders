using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Organizations;
using TechRiders.Api.Contracts.Responses.Organizations;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Organizaciones (centro, empresa, comunidad, etc.) y su relación con personas (Requisitos Arquitectura §6).</summary>
[ApiController]
[Route("api/organizations")]
[Produces("application/json")]
[Authorize]
public sealed class OrganizationsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public OrganizationsController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrganizationResponse>))]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var organizations = await OrganizationService.ListAsync(_dbContext, cancellationToken);
        return Ok(organizations.Select(o => new OrganizationResponse
        {
            Id = o.Id,
            OrganizationType = o.OrganizationType.ToString(),
            Name = o.Name,
            TaxId = o.TaxId,
            Website = o.Website,
            Address = o.Address,
            Province = o.Province,
            IsActive = o.IsActive,
        }));
    }

    [HttpPost]
    [Authorize(Policy = "permission:community.manage")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var organization = await OrganizationService.CreateAsync(_dbContext, request.OrganizationType, request.Name, request.TaxId, request.Website, request.Address, request.Province, cancellationToken);
            return CreatedAtAction(nameof(List), null, new { organization.Id });
        }
        catch (ArgumentException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("relations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestRelation([FromBody] RequestPersonOrganizationRequest request, CancellationToken cancellationToken)
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
            var relation = await OrganizationService.RequestRelationAsync(_dbContext, userId.Value, request.OrganizationId, request.RelationType, request.Position, cancellationToken);
            return CreatedAtAction(nameof(GetPendingRelations), null, new { relation.Id });
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpGet("relations/pending")]
    [Authorize(Policy = "permission:validations.manage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonOrganizationResponse>))]
    public async Task<IActionResult> GetPendingRelations(CancellationToken cancellationToken)
    {
        var pending = await OrganizationService.GetPendingRelationsAsync(_dbContext, cancellationToken);
        return Ok(pending.Select(ToResponse));
    }

    [HttpPost("relations/{id:guid}/approve")]
    [Authorize(Policy = "permission:validations.manage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveRelation(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            var relation = await OrganizationService.ApproveRelationAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok(ToResponse(relation));
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("relations/{id:guid}/reject")]
    [Authorize(Roles = "Admin,Staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectRelation(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            var relation = await OrganizationService.RejectRelationAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok(ToResponse(relation));
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

    private static PersonOrganizationResponse ToResponse(Domain.Entities.PersonOrganization relation) => new()
    {
        Id = relation.Id,
        UserId = relation.UserId,
        UserName = relation.User is null ? null : $"{relation.User.Name} {relation.User.LastName}",
        OrganizationId = relation.OrganizationId,
        OrganizationName = relation.Organization?.Name ?? string.Empty,
        Position = relation.Position,
        RelationType = relation.RelationType.ToString(),
        Status = relation.Status.ToString(),
        IsPrimaryContact = relation.IsPrimaryContact,
        RequestedAt = relation.CreatedAt,
        ValidatedAt = relation.ValidatedAt,
    };
}
