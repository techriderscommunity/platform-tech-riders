using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Consents;
using TechRiders.Api.Contracts.Responses.Consents;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Consentimientos por finalidad (Requisitos Arquitectura §8). Un interés/preferencia NUNCA es prueba de consentimiento.</summary>
[ApiController]
[Route("api/consents")]
[Produces("application/json")]
[Authorize]
public sealed class ConsentsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public ConsentsController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("purposes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsentPurposeResponse>))]
    public async Task<IActionResult> GetPurposes(CancellationToken cancellationToken)
    {
        var purposes = await ConsentService.GetPurposesAsync(_dbContext, cancellationToken);
        return Ok(purposes.Select(p => new ConsentPurposeResponse { Id = p.Id, Code = p.Code, Name = p.Name, Description = p.Description }));
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsentResponse>))]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var consents = await ConsentService.GetUserConsentsAsync(_dbContext, userId.Value, cancellationToken);
        return Ok(consents.Select(ToResponse));
    }

    [HttpPost("me/grant")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConsentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Grant([FromBody] ConsentPurposeCodeRequest request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var consent = await ConsentService.GrantAsync(_dbContext, userId.Value, request.PurposeCode, cancellationToken);
            return Ok(ToResponse(consent));
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("me/withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConsentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdraw([FromBody] ConsentPurposeCodeRequest request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var consent = await ConsentService.WithdrawAsync(_dbContext, userId.Value, request.PurposeCode, cancellationToken);
            return Ok(ToResponse(consent));
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

    private static ConsentResponse ToResponse(Domain.Entities.Consent consent) => new()
    {
        PurposeId = consent.PurposeId,
        PurposeCode = consent.Purpose?.Code ?? string.Empty,
        PurposeName = consent.Purpose?.Name ?? string.Empty,
        Status = consent.Status.ToString(),
        GrantedAt = consent.GrantedAt,
        WithdrawnAt = consent.WithdrawnAt,
    };
}
