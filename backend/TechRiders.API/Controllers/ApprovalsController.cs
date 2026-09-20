using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.Approvals;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Bandeja centralizada de elementos pendientes de revisi\u00f3n (Requisitos: Bandeja de Aprobaciones).</summary>
[ApiController]
[Route("api/approvals")]
[Produces("application/json")]
[Authorize(Policy = "permission:approvals.manage")]
public sealed class ApprovalsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public ApprovalsController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApprovalItemResponse>))]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var items = await ApprovalsAggregationService.GetPendingAsync(_dbContext, cancellationToken);
        return Ok(items.Select(i => new ApprovalItemResponse
        {
            Id = i.Id,
            Type = i.Type,
            Title = i.Title,
            RequestedBy = i.RequestedBy,
            RequestedAt = i.RequestedAt,
            Module = i.Module,
        }));
    }
}
