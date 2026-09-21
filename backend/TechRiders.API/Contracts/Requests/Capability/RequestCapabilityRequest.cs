using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Capability;

public sealed class RequestCapabilityRequest
{
    /// <summary>Nombre exacto del rol solicitado: Staff, Community Leader, Ambassador, Center o Community Partner.</summary>
    [Required]
    public required string CapabilityName { get; set; }
}
