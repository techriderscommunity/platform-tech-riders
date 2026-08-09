using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveTraceRequest
{
    [Required]
    [StringLength(80)]
    public string Kind { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Route { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Detail { get; set; } = string.Empty;
}