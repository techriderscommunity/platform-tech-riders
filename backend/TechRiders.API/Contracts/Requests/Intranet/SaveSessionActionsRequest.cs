using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveSessionActionsRequest
{
    [StringLength(254)]
    public string? UserKey { get; set; }

    public Dictionary<string, SaveSessionActionItem> Actions { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}