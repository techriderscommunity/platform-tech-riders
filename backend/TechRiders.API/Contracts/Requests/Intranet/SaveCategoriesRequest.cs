using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveCategoriesRequest
{
    [StringLength(254)]
    public string? UserKey { get; set; }

    public List<string> Categories { get; set; } = [];
}