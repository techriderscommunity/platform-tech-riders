using MapsterMapper;
using TechRiders.Application.DTOs.Requests.Knowledge;
using TechRiders.Application.DTOs.Responses.Knowledge;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Coordinates the tutorial catalog flow and keeps the repository-based
/// persistence logic isolated from the HTTP and DTO layers.
/// </summary>
public class TutorialsService : ITutorialsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TutorialsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TutorialResponse>> GetAllTutorialsAsync(CancellationToken cancellationToken = default)
    {
        var tutorials = await _unitOfWork.Tutoriales.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TutorialResponse>>(tutorials);
    }

    public async Task<TutorialResponse?> GetTutorialByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tutorial = await _unitOfWork.Tutoriales.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<TutorialResponse?>(tutorial);
    }

    public async Task<TutorialResponse?> GetTutorialBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var tutorial = await _unitOfWork.Tutoriales.GetBySlugAsync(slug, cancellationToken);
        return _mapper.Map<TutorialResponse?>(tutorial);
    }

    public async Task<IEnumerable<TutorialResponse>> GetTutorialsByAuthorAsync(string author, CancellationToken cancellationToken = default)
    {
        var tutorials = await _unitOfWork.Tutoriales.GetByAutorAsync(author, cancellationToken);
        return _mapper.Map<IEnumerable<TutorialResponse>>(tutorials);
    }

    public async Task<IEnumerable<TutorialResponse>> GetTutorialsByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var tutorials = await _unitOfWork.Tutoriales.GetByCategoriaAsync(category, cancellationToken);
        return _mapper.Map<IEnumerable<TutorialResponse>>(tutorials);
    }

    public async Task<(IEnumerable<TutorialResponse> Items, int TotalCount)> GetPaginatedTutorialsAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.Tutoriales.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var mappedItems = _mapper.Map<IEnumerable<TutorialResponse>>(pagedResult.Items);

        return (mappedItems, pagedResult.TotalCount);
    }

    public async Task<TutorialResponse> CreateTutorialAsync(CreateTutorialRequest request, CancellationToken cancellationToken = default)
    {
        var slugExists = await _unitOfWork.Tutoriales.SlugExistsAsync(request.Slug, null, cancellationToken);
        if (slugExists)
            throw new InvalidOperationException($"Slug '{request.Slug}' is already in use");

        var tutorial = Tutorial.Create(
            slug: request.Slug,
            titulo: request.Titulo,
            extracto: request.Extracto,
            autor: request.Autor,
            categoriasJson: request.CategoriasJson,
            url: request.Url
        );

        await _unitOfWork.Tutoriales.AddAsync(tutorial, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TutorialResponse>(tutorial);
    }

    public async Task<TutorialResponse> UpdateTutorialAsync(UpdateTutorialRequest request, CancellationToken cancellationToken = default)
    {
        var tutorial = await _unitOfWork.Tutoriales.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tutorial {request.Id} not found");

        if (!string.IsNullOrEmpty(request.Titulo)) tutorial.Titulo = request.Titulo;
        if (!string.IsNullOrEmpty(request.Extracto)) tutorial.Extracto = request.Extracto;
        if (!string.IsNullOrEmpty(request.CategoriasJson)) tutorial.CategoriasJson = request.CategoriasJson;
        if (!string.IsNullOrEmpty(request.Url)) tutorial.Url = request.Url;

        await _unitOfWork.Tutoriales.UpdateAsync(tutorial, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TutorialResponse>(tutorial);
    }

    public async Task DeleteTutorialAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tutorial = await _unitOfWork.Tutoriales.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tutorial {id} not found");

        await _unitOfWork.Tutoriales.DeleteAsync(tutorial, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Tutoriales.SlugExistsAsync(slug, excludeId, cancellationToken);
    }
}
