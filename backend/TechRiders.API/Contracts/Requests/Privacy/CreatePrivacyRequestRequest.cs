using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Privacy;

public sealed class CreatePrivacyRequestRequest
{
    [Required]
    public required string RequestType { get; set; }

    public string? Channel { get; set; }
}
