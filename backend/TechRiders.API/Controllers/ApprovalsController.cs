using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.Approvals;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Bandeja centralizada de elementos pendientes de revisi\u00f3n (Requisitos: Bandeja de Aprobaciones).</summary>
[ApiController]
[Route("api/approvals")]
[Produces("application/json")]
[Authorize(Policy = "permission:approvals.manage")]
public sealed class ApprovalsController : BaseApiController
{
    private readonly IApprovalsService _approvalsService;

    public ApprovalsController(IApprovalsService approvalsService)
    {
        _approvalsService = approvalsService;
    }

    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApprovalItemResponse>))]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var items = await _approvalsService.GetPendingAsync(cancellationToken);
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
