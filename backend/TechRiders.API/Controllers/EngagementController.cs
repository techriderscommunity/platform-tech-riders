using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Engagement;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class EngagementController : BaseApiController
{
    private readonly ILogger<EngagementController> _logger;
    private readonly TechRidersDbContext _dbContext;

    public EngagementController(ILogger<EngagementController> logger, TechRidersDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost("contact")]
    [HttpPost("contacto")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SubmitContact([FromBody] ContactRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _logger.LogInformation("Contact message received from {Name}<{Email}>", request.Name, request.Email);
        return Accepted(new { success = true, message = "Contact message received" });
    }

    [HttpPost("suggestions")]
    [HttpPost("sugerencias")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SubmitSuggestion([FromBody] SuggestionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _logger.LogInformation("Suggestion received from {Name}", request.Name);
        return Accepted(new { success = true, message = "Suggestion received" });
    }

    [HttpPost("join")]
    [HttpPost("join/member")]
    [HttpPost("join/ambassador")]
    [HttpPost("join/session")]
    [HttpPost("sessions/request")]
    [HttpPost("ambassadors/apply")]
    [HttpPost("solicitudes/candidato")]
    [HttpPost("solicitudes/centro")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SubmitJoin([FromBody] JoinRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _logger.LogInformation(
            "Public intake request received from {Name}<{Email}>. Type: {RequestType}; CommunityRole: {CommunityRole}; Audience: {Audience}",
            request.Name,
            request.Email,
            request.RequestType,
            request.CommunityRole,
            request.Audience);

        // Las solicitudes de Centro generan un registro pendiente real (visible en la Bandeja de
        // Aprobaciones), en vez de quedar solo como log/lead.
        if (Request.Path.Value?.EndsWith("solicitudes/centro", StringComparison.OrdinalIgnoreCase) == true)
        {
            var organizationName = string.IsNullOrWhiteSpace(request.Organization) ? request.Name : request.Organization;
            var contactInfo = $"Solicitante: {request.Name} <{request.Email}>. {request.Motivation}";
            await OrganizationService.CreatePendingAsync(_dbContext, "CentroEducativo", organizationName, contactInfo, null, cancellationToken);
        }

        return Accepted(new
        {
            success = true,
            message = "Join request received and queued for review.",
        });
    }
}