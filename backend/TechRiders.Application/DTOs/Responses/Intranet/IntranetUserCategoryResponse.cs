namespace TechRiders.Application.DTOs.Responses.Intranet;

/// <summary>
/// Response model for intranet user category
/// </summary>
public class IntranetUserCategoryResponse
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// Category description
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Whether this category is active for the user
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsActive { get; set; }
}
