using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Category;
using TechRiders.Application.DTOs.Responses.Category;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;

namespace TechRiders.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Categories.GetCategoryWithSubCategoriesAsync(id, cancellationToken);
        if (category == null || !category.Active) return null;
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<IEnumerable<CategoryResponse>> GetMainCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetMainCategoriesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<IEnumerable<CategoryResponse>> GetSubCategoriesAsync(int fatherId, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetSubCategoriesAsync(fatherId, cancellationToken);
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        if (request.FatherId.HasValue)
        {
            var fatherExists = await _unitOfWork.Categories.ExistsAsync(request.FatherId.Value, cancellationToken);
            if (!fatherExists) throw new InvalidOperationException($"Father category with ID {request.FatherId.Value} does not exist");
        }

        var category = _mapper.Map<MT_Category>(request);
        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        if (category == null || !category.Active) return null;

        if (request.FatherId.HasValue)
        {
            if (request.FatherId.Value == id)
                throw new InvalidOperationException("A category cannot be its own father");

            var fatherExists = await _unitOfWork.Categories.ExistsAsync(request.FatherId.Value, cancellationToken);
            if (!fatherExists) throw new InvalidOperationException($"Father category with ID {request.FatherId.Value} does not exist");
        }

        _mapper.Map(request, category);
        await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        if (category == null || !category.Active) return false;

        category.Active = false;
        await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
