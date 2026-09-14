using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Consents;

public sealed class ConsentPurposeCodeRequest
{
    [Required]
    public required string PurposeCode { get; set; }
}
