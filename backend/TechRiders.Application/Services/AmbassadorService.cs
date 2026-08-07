using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Ambassador;
using TechRiders.Application.DTOs.Responses.Ambassador;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Servicio de aplicación para gestión de ambassadors
/// Implementa la lógica de negocio y orquesta operaciones del dominio
/// </summary>
public class AmbassadorService : IAmbassadorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AmbassadorService> _logger;

    public AmbassadorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AmbassadorService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetAllAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all active ambassadors");

        var ambassadors = await _unitOfWork.Ambassadors.GetActiveAmbassadorsAsync(cancellationToken);
        return ambassadors.Adapt<IEnumerable<AmbassadorResponse>>();
    }

    public async Task<AmbassadorResponse?> GetAmbassadorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting ambassador with ID: {AmbassadorId}", id);

        var ambassador = await _unitOfWork.Ambassadors.GetAmbassadorWithCategoryAsync(id, cancellationToken);

        if (ambassador == null || !ambassador.IsActive)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or inactive", id);
            return null;
        }

        return _mapper.Map<AmbassadorResponse>(ambassador);
    }

    public async Task<IEnumerable<AmbassadorResponse>> SearchAmbassadorsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching ambassadors with term: {SearchTerm}", searchTerm);

        var ambassadors = await _unitOfWork.Ambassadors.SearchAmbassadorsAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<AmbassadorResponse>>(ambassadors);
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetAmbassadorsByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting ambassadors by category ID: {CategoryId}", categoryId);

        var ambassadors = await _unitOfWork.Ambassadors.GetAmbassadorsByCategoryAsync(categoryId, cancellationToken);
        return _mapper.Map<IEnumerable<AmbassadorResponse>>(ambassadors);
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting working ambassadors");

        var ambassadors = await _unitOfWork.Ambassadors.GetWorkingAmbassadorsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AmbassadorResponse>>(ambassadors);
    }

    public async Task<AmbassadorResponse> CreateAmbassadorAsync(
        CreateAmbassadorRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new ambassador: {Name} {LastName}", request.Name, request.LastName);

        // Validar categoría si se proporciona
        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(request.CategoryId.Value, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category with ID {CategoryId} does not exist", request.CategoryId.Value);
                throw new InvalidOperationException($"Category with ID {request.CategoryId.Value} does not exist");
            }
        }

        var ambassador = _mapper.Map<Ambassador>(request);

        await _unitOfWork.Ambassadors.AddAsync(ambassador, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador created successfully with ID: {AmbassadorId}", ambassador.Id);

        return _mapper.Map<AmbassadorResponse>(ambassador);
    }

    public async Task<AmbassadorResponse?> UpdateAmbassadorAsync(
        Guid id,
        UpdateAmbassadorRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating ambassador with ID: {AmbassadorId}", id);

        var ambassador = await _unitOfWork.Ambassadors.GetByIdAsync(id, cancellationToken);

        if (ambassador == null || !ambassador.IsActive)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or inactive", id);
            return null;
        }

        // Validar categoría si se proporciona
        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(request.CategoryId.Value, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category with ID {CategoryId} does not exist", request.CategoryId.Value);
                throw new InvalidOperationException($"Category with ID {request.CategoryId.Value} does not exist");
            }
        }

        _mapper.Map(request, ambassador);
        ambassador.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Ambassadors.UpdateAsync(ambassador, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador updated successfully: {AmbassadorId}", id);

        return _mapper.Map<AmbassadorResponse>(ambassador);
    }

    public async Task<bool> DeleteAmbassadorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting ambassador with ID: {AmbassadorId}", id);

        var ambassador = await _unitOfWork.Ambassadors.GetByIdAsync(id, cancellationToken);

        if (ambassador == null || !ambassador.IsActive)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or already inactive", id);
            return false;
        }

        ambassador.IsActive = false;
        ambassador.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Ambassadors.UpdateAsync(ambassador, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador deleted successfully: {AmbassadorId}", id);

        return true;
    }
}
