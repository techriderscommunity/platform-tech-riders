using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Intranet;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/intranet")]
[Produces("application/json")]
public sealed class IntranetOperationsController : ControllerBase
{
    private readonly IIntranetRuntimeOperationsService _runtimeOperationsService;

    public IntranetOperationsController(IIntranetRuntimeOperationsService runtimeOperationsService)
    {
        _runtimeOperationsService = runtimeOperationsService ?? throw new ArgumentNullException(nameof(runtimeOperationsService));
    }

    [HttpPost("trazas")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveTrace([FromBody] SaveTraceRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _runtimeOperationsService.SaveTrace(request);

        return Accepted(new { success = true });
    }

    [HttpGet("session-actions")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDictionary<string, SessionActionState>))]
    public IActionResult GetSessionActions([FromQuery] string? userKey)
    {
        var actions = _runtimeOperationsService.GetSessionActions();
        return Ok(actions);
    }

    [HttpPut("session-actions")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveSessionActions([FromBody] SaveSessionActionsRequest request)
    {
        try
        {
            var savedActions = _runtimeOperationsService.SaveSessionActions(request);
            return Ok(savedActions);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}