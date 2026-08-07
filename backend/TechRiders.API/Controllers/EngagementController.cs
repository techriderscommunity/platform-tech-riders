using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class EngagementController : ControllerBase
{
    private readonly ILogger<EngagementController> _logger;

    public EngagementController(ILogger<EngagementController> logger)
    {
        _logger = logger;
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

        _logger.LogInformation("Join request received from {Name} as {Role}", request.Name, request.Role);
        return Accepted(new { success = true, message = "Join request received" });
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
    [StringLength(50)]
    public string Role { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Organization { get; set; }

    [Required]
    [StringLength(2000)]
    public string Motivation { get; set; } = string.Empty;
}