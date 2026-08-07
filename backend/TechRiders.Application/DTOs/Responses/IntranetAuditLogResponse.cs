namespace TechRiders.Application.DTOs.Responses;

/// <summary>
/// Response model for intranet audit log
/// </summary>
public class IntranetAuditLogResponse
{
    /// <summary>
    /// Audit log unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// UTC timestamp of creation
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Actor user ID (nullable for system actions)
    /// </summary>
    public Guid? ActorUserId { get; set; }

    /// <summary>
    /// Actor email address
    /// </summary>
    public required string ActorEmail { get; set; }

    /// <summary>
    /// Module or feature name
    /// </summary>
    public required string Module { get; set; }

    /// <summary>
    /// Action performed
    /// </summary>
    public required string Action { get; set; }

    /// <summary>
    /// Result of the action (Success, Failure, etc)
    /// </summary>
    public required string Result { get; set; }

    /// <summary>
    /// Additional details about the action
    /// </summary>
    public string? Detail { get; set; }
}
