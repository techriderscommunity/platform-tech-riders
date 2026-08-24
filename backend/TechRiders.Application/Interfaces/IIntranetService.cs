using TechRiders.Application.DTOs.Requests.Intranet;
using TechRiders.Application.DTOs.Responses.Intranet;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Service interface for intranet operations (audit logs, settings, user categories)
/// </summary>
public interface IIntranetService
{
    // Audit log operations

    /// <summary>
    /// Gets all audit logs
    /// </summary>
    Task<IEnumerable<IntranetAuditLogResponse>> GetAllAuditLogsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by module
    /// </summary>
    Task<IEnumerable<IntranetAuditLogResponse>> GetAuditLogsByModuleAsync(string module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by actor user ID
    /// </summary>
    Task<IEnumerable<IntranetAuditLogResponse>> GetAuditLogsByActorAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets filtered audit logs
    /// </summary>
    Task<IEnumerable<IntranetAuditLogResponse>> GetFilteredAuditLogsAsync(
        string? module = null,
        string? action = null,
        Guid? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    // Setting operations

    /// <summary>
    /// Gets all intranet settings
    /// </summary>
    Task<IEnumerable<IntranetSettingResponse>> GetAllSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a setting by key
    /// </summary>
    Task<IntranetSettingResponse?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets settings by module
    /// </summary>
    Task<IEnumerable<IntranetSettingResponse>> GetSettingsByModuleAsync(string module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a setting by module and key
    /// </summary>
    Task<IntranetSettingResponse?> GetSettingByModuleAndKeyAsync(string module, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing intranet setting.
    /// </summary>
    Task<IntranetSettingResponse?> UpdateSettingAsync(UpdateIntranetSettingRequest request, string? updatedBy, CancellationToken cancellationToken = default);

    // User category operations

    /// <summary>
    /// Gets all user categories for a user
    /// </summary>
    Task<IEnumerable<IntranetUserCategoryResponse>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets only active user categories
    /// </summary>
    Task<IEnumerable<IntranetUserCategoryResponse>> GetActiveUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user has a specific category
    /// </summary>
    Task<bool> UserHasCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default);
}
