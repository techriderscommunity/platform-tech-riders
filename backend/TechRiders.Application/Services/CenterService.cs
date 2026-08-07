using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Center;
using TechRiders.Application.DTOs.Responses.Center;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;

namespace TechRiders.Application.Services;

public class CenterService : ICenterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CenterService> _logger;

    public CenterService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CenterService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CenterResponse>> GetAllCentersAsync(CancellationToken cancellationToken = default)
    {
        var centers = await _unitOfWork.Centers.GetActiveCentersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CenterResponse>>(centers);
    }

    public async Task<CenterResponse?> GetCenterByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var center = await _unitOfWork.Centers.GetByIdAsync(id, cancellationToken);
        if (center == null || !center.IsActive) return null;
        return _mapper.Map<CenterResponse>(center);
    }

    public async Task<IEnumerable<CenterResponse>> SearchCentersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var centers = await _unitOfWork.Centers.SearchCentersAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<CenterResponse>>(centers);
    }

    public async Task<IEnumerable<CenterResponse>> GetCentersByLocalityAsync(string locality, CancellationToken cancellationToken = default)
    {
        var centers = await _unitOfWork.Centers.GetCentersByLocalityAsync(locality, cancellationToken);
        return _mapper.Map<IEnumerable<CenterResponse>>(centers);
    }

    public async Task<CenterResponse> CreateCenterAsync(CreateCenterRequest request, CancellationToken cancellationToken = default)
    {
        var center = _mapper.Map<Center>(request);
        await _unitOfWork.Centers.AddAsync(center, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CenterResponse>(center);
    }

    public async Task<CenterResponse?> UpdateCenterAsync(Guid id, UpdateCenterRequest request, CancellationToken cancellationToken = default)
    {
        var center = await _unitOfWork.Centers.GetByIdAsync(id, cancellationToken);
        if (center == null || !center.IsActive) return null;

        _mapper.Map(request, center);
        center.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Centers.UpdateAsync(center, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CenterResponse>(center);
    }

    public async Task<bool> DeleteCenterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var center = await _unitOfWork.Centers.GetByIdAsync(id, cancellationToken);
        if (center == null || !center.IsActive) return false;

        center.IsActive = false;
        center.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Centers.UpdateAsync(center, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
