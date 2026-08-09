using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveSessionActionItem
{
    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(80)]
    public string? AmbassadorAssignedId { get; set; }
}