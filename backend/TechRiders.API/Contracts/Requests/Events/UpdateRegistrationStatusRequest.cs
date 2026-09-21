using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Events;

public sealed class UpdateRegistrationStatusRequest
{
    /// <summary>Registered, WaitingList, Cancelled, Attended o NoShow.</summary>
    [Required]
    public required string Status { get; set; }
}
