using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class EngagementController : ControllerBase
{
    private readonly ILogger<EngagementController> _logger;
    private readonly IMvpRuntimeStateStore mvpRuntimeStateStore;

    private static readonly HashSet<string> AllowedRequestTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "member",
        "ambassador",
        "session"
    };

    public EngagementController(ILogger<EngagementController> logger, IMvpRuntimeStateStore mvpRuntimeStateStore)
    {
        _logger = logger;
        this.mvpRuntimeStateStore = mvpRuntimeStateStore;
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
    public ActionResult SubmitJoin([FromBody] JoinRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!AllowedRequestTypes.Contains(request.RequestType))
        {
            ModelState.AddModelError(nameof(request.RequestType), "RequestType must be member, ambassador or session.");
            return ValidationProblem(ModelState);
        }

        _logger.LogInformation(
            "Public intake request received from {Name}<{Email}>. Type: {RequestType}; CommunityRole: {CommunityRole}; Audience: {Audience}",
            request.Name,
            request.Email,
            request.RequestType,
            request.CommunityRole,
            request.Audience);

        if (string.Equals(request.RequestType, "member", StringComparison.OrdinalIgnoreCase))
        {
            mvpRuntimeStateStore.UpsertMemberProfile(request.Email, new MemberProfileState
            {
                Name = request.Name,
                Email = request.Email,
                Bio = request.Motivation,
                Interests = request.Audience ?? string.Empty,
                Audience = request.Audience ?? "junior",
                CommunityRole = request.CommunityRole,
                Organization = request.Organization ?? string.Empty,
            });
        }

        if (string.Equals(request.RequestType, "ambassador", StringComparison.OrdinalIgnoreCase))
        {
            mvpRuntimeStateStore.UpsertAmbassadorPortal(request.Email, new AmbassadorPortalState
            {
                Email = request.Email,
                Bio = request.Motivation,
                Specialties = string.Join(" · ", new[] { request.Audience, request.Organization }.Where(value => !string.IsNullOrWhiteSpace(value))),
                Availability = "Pendiente de completar por el ambassador en intranet",
            });
        }

        return Accepted(new
        {
            success = true,
            message = request.RequestType switch
            {
                "member" => "Member application received",
                "ambassador" => "Ambassador application received",
                "session" => "Session request received",
                _ => "Request received"
            }
        });
    }
}

public sealed class ContactRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
}

public sealed class SuggestionRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Text { get; set; } = string.Empty;
}

public sealed class JoinRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string RequestType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CommunityRole { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Audience { get; set; }

    [StringLength(200)]
    public string? Organization { get; set; }

    [StringLength(150)]
    public string? SessionTopic { get; set; }

    [StringLength(80)]
    public string? SessionFormat { get; set; }

    [Required]
    [StringLength(2000)]
    public string Motivation { get; set; } = string.Empty;
}