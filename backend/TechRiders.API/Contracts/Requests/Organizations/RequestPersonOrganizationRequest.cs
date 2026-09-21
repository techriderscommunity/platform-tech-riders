using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Organizations;

public sealed class RequestPersonOrganizationRequest
{
    [Required]
    public required Guid OrganizationId { get; set; }

    [Required]
    public required string RelationType { get; set; }

    public string? Position { get; set; }
}
