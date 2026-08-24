using MapsterMapper;
using TechRiders.Application.DTOs.Requests.Intranet;
using TechRiders.Application.DTOs.Responses.Intranet;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Coordinates intranet concerns such as audit history, system settings, and user
/// category checks while delegating persistence to the unit-of-work abstraction.
/// </summary>
public class IntranetService : IIntranetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IntranetService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<IntranetAuditLogResponse>> GetAllAuditLogsAsync(CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.IntranetAuditLogs.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<IntranetAuditLogResponse>>(logs);
    }

    public async Task<IEnumerable<IntranetAuditLogResponse>> GetAuditLogsByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.IntranetAuditLogs.GetByModuleAsync(module, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetAuditLogResponse>>(logs);
    }

    public async Task<IEnumerable<IntranetAuditLogResponse>> GetAuditLogsByActorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.IntranetAuditLogs.GetByActorUserIdAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetAuditLogResponse>>(logs);
    }

    public async Task<IEnumerable<IntranetAuditLogResponse>> GetFilteredAuditLogsAsync(
        string? module = null,
        string? action = null,
        Guid? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.IntranetAuditLogs.GetFilteredAsync(module, action, userId, startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetAuditLogResponse>>(logs);
    }

    public async Task<IEnumerable<IntranetSettingResponse>> GetAllSettingsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _unitOfWork.IntranetSettings.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<IntranetSettingResponse>>(settings);
    }

    public async Task<IntranetSettingResponse?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var setting = await _unitOfWork.IntranetSettings.GetByKeyAsync(key, cancellationToken);
        return _mapper.Map<IntranetSettingResponse?>(setting);
    }

    public async Task<IEnumerable<IntranetSettingResponse>> GetSettingsByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        var settings = await _unitOfWork.IntranetSettings.GetByModuleAsync(module, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetSettingResponse>>(settings);
    }

    public async Task<IntranetSettingResponse?> GetSettingByModuleAndKeyAsync(string module, string key, CancellationToken cancellationToken = default)
    {
        var setting = await _unitOfWork.IntranetSettings.GetByModuleAndKeyAsync(module, key, cancellationToken);
        return _mapper.Map<IntranetSettingResponse?>(setting);
    }

    public async Task<IntranetSettingResponse?> UpdateSettingAsync(UpdateIntranetSettingRequest request, string? updatedBy, CancellationToken cancellationToken = default)
    {
        var existingSetting = await _unitOfWork.IntranetSettings.GetByKeyAsync(request.Key, cancellationToken);
        if (existingSetting is null)
        {
            return null;
        }

        existingSetting.Update(request.Module, request.Value, request.Status, updatedBy);
        await _unitOfWork.IntranetSettings.UpdateAsync(existingSetting, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<IntranetSettingResponse>(existingSetting);
    }

    public async Task<IEnumerable<IntranetUserCategoryResponse>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.IntranetUserCategories.GetByUserIdAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetUserCategoryResponse>>(categories);
    }

    public async Task<IEnumerable<IntranetUserCategoryResponse>> GetActiveUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.IntranetUserCategories.GetActiveByUserIdAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<IntranetUserCategoryResponse>>(categories);
    }

    public async Task<bool> UserHasCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.IntranetUserCategories.UserHasCategoryAsync(userId, category, cancellationToken);
    }
}
