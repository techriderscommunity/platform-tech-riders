using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Organizations;

public sealed class UpdateOrganizationRequest
{
    [Required]
    public required string Name { get; set; }

    public string? TaxId { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? Province { get; set; }
    public string? Notes { get; set; }
}
