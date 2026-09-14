using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Privacy;

public sealed class ResolvePrivacyRequestRequest
{
    [Required]
    public required string Status { get; set; }

    public string? Resolution { get; set; }
}
