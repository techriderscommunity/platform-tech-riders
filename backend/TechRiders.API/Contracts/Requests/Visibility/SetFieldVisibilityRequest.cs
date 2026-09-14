using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Visibility;

public sealed class SetFieldVisibilityRequest
{
    [Required]
    public required string FieldKey { get; set; }

    [Required]
    public required string Visibility { get; set; }
}
