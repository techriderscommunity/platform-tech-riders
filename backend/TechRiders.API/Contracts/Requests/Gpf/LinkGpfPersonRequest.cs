using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Gpf;

public sealed class LinkGpfPersonRequest
{
    [Required]
    public required Guid UserId { get; set; }

    [Required]
    public required string CodUnico { get; set; }
}
