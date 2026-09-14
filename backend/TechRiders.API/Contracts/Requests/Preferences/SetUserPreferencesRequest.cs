using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Preferences;

public sealed class SetUserPreferencesRequest
{
    [Required]
    public required List<Guid> DimensionValueIds { get; set; }
}
