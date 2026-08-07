using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.FPTour;
using TechRiders.Application.DTOs.Responses.FPTour;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;
namespace TechRiders.Application.Services;

public class FPTourService : IFPTourService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;   
    private readonly ILogger<FPTourService> _logger;

    public FPTourService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FPTourService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<FPTourResponse>> GetAllFPToursAsync(CancellationToken cancellationToken = default)
    {
        var tours = await _unitOfWork.FPTours.GetActiveFPToursAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FPTourResponse>>(tours);
    }

    public async Task<FPTourResponse?> GetFPTourByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tour = await _unitOfWork.FPTours.GetFPTourWithDetailsAsync(id, cancellationToken);
        if (tour == null || !tour.IsActive) return null;
        return _mapper.Map<FPTourResponse>(tour);
    }

    public async Task<IEnumerable<FPTourResponse>> GetFPToursByCenterAsync(Guid centerId, CancellationToken cancellationToken = default)
    {
        var tours = await _unitOfWork.FPTours.GetFPToursByCenterAsync(centerId, cancellationToken);
        return _mapper.Map<IEnumerable<FPTourResponse>>(tours);
    }

    public async Task<IEnumerable<FPTourResponse>> GetFPToursByAmbassadorAsync(Guid ambassadorId, CancellationToken cancellationToken = default)
    {
        var tours = await _unitOfWork.FPTours.GetFPToursByAmbassadorAsync(ambassadorId, cancellationToken);
        return _mapper.Map<IEnumerable<FPTourResponse>>(tours);
    }

    public async Task<IEnumerable<FPTourResponse>> GetPendingFPToursAsync(CancellationToken cancellationToken = default)
    {
        var tours = await _unitOfWork.FPTours.GetPendingFPToursAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FPTourResponse>>(tours);
    }

    public async Task<FPTourResponse> CreateFPTourAsync(CreateFPTourRequest request, CancellationToken cancellationToken = default)
    {
        // Validar que center y ambassador existan
        var centerExists = await _unitOfWork.Centers.ExistsAsync(c => c.Id == request.CenterId && c.IsActive, cancellationToken);
        var ambassadorExists = await _unitOfWork.Ambassadors.ExistsAsync(a => a.Id == request.AmbassadorId && a.IsActive, cancellationToken);

        if (!centerExists) throw new InvalidOperationException($"Center with ID {request.CenterId} does not exist");
        if (!ambassadorExists) throw new InvalidOperationException($"Ambassador with ID {request.AmbassadorId} does not exist");

        var tour = _mapper.Map<FPTour>(request);
        await _unitOfWork.FPTours.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FPTourResponse>(await _unitOfWork.FPTours.GetFPTourWithDetailsAsync(tour.Id, cancellationToken));
    }

    public async Task<FPTourResponse?> UpdateFPTourAsync(Guid id, UpdateFPTourRequest request, CancellationToken cancellationToken = default)
    {
        var tour = await _unitOfWork.FPTours.GetByIdAsync(id, cancellationToken);
        if (tour == null || !tour.IsActive) return null;

        if (request.CenterId.HasValue)
        {
            var centerExists = await _unitOfWork.Centers.ExistsAsync(c => c.Id == request.CenterId.Value && c.IsActive, cancellationToken);
            if (!centerExists) throw new InvalidOperationException($"Center with ID {request.CenterId.Value} does not exist");
        }

        if (request.AmbassadorId.HasValue)
        {
            var ambassadorExists = await _unitOfWork.Ambassadors.ExistsAsync(a => a.Id == request.AmbassadorId.Value && a.IsActive, cancellationToken);
            if (!ambassadorExists) throw new InvalidOperationException($"Ambassador with ID {request.AmbassadorId.Value} does not exist");
        }

        _mapper.Map(request, tour);
        tour.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.FPTours.UpdateAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FPTourResponse>(await _unitOfWork.FPTours.GetFPTourWithDetailsAsync(tour.Id, cancellationToken));
    }

    public async Task<bool> DeleteFPTourAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tour = await _unitOfWork.FPTours.GetByIdAsync(id, cancellationToken);
        if (tour == null || !tour.IsActive) return false;

        tour.IsActive = false;
        tour.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.FPTours.UpdateAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
