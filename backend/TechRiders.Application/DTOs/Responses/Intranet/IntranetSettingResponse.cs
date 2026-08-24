namespace TechRiders.Application.DTOs.Responses.Intranet;

/// <summary>
/// Response model for intranet setting
/// </summary>
public class IntranetSettingResponse
{
    /// <summary>
    /// Setting unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Setting key/name
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Module name
    /// </summary>
    public required string Module { get; set; }

    /// <summary>
    /// Setting value
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// Setting status (Active, Inactive, etc)
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime UpdatedUtc { get; set; }

    /// <summary>
    /// User who performed the last update
    /// </summary>
    public required string UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsActive { get; set; }
}
