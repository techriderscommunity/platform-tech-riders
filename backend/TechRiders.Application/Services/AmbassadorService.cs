using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Ambassador;
using TechRiders.Application.DTOs.Responses.Ambassador;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Orchestrates ambassador flows while keeping business logic isolated from
/// persistence and HTTP concerns.
/// </summary>
public class AmbassadorService : IAmbassadorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AmbassadorService> _logger;

    public AmbassadorService(
        IUnitOfWork unitOfWork,
        ILogger<AmbassadorService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetAllAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all active ambassadors");

        var ambassadors = await _unitOfWork.Ambassadors.GetActiveAmbassadorsAsync(cancellationToken);
        return await MapUsersToResponsesAsync(ambassadors, cancellationToken);
    }

    public async Task<AmbassadorResponse?> GetAmbassadorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting ambassador with ID: {AmbassadorId}", id);

        var ambassador = await _unitOfWork.Ambassadors.GetAmbassadorWithDetailsAsync(id, cancellationToken);

        if (ambassador == null)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or inactive", id);
            return null;
        }

        var responses = await MapUsersToResponsesAsync([ambassador], cancellationToken);
        return responses.FirstOrDefault();
    }

    public async Task<IEnumerable<AmbassadorResponse>> SearchAmbassadorsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching ambassadors with term: {SearchTerm}", searchTerm);

        var ambassadors = await _unitOfWork.Ambassadors.SearchAmbassadorsAsync(searchTerm, cancellationToken);
        return await MapUsersToResponsesAsync(ambassadors, cancellationToken);
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetAmbassadorsByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting ambassadors by category ID: {CategoryId}", categoryId);

        var ambassadors = await _unitOfWork.Ambassadors.GetAmbassadorsByCategoryAsync(categoryId, cancellationToken);
        return await MapUsersToResponsesAsync(ambassadors, cancellationToken);
    }

    public async Task<IEnumerable<AmbassadorResponse>> GetWorkingAmbassadorsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting working ambassadors");

        var ambassadors = await _unitOfWork.Ambassadors.GetWorkingAmbassadorsAsync(cancellationToken);
        return await MapUsersToResponsesAsync(ambassadors, cancellationToken);
    }

    public async Task<AmbassadorResponse> CreateAmbassadorAsync(
        CreateAmbassadorRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new ambassador: {Name} {LastName}", request.Name, request.LastName);

        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(request.CategoryId.Value, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category with ID {CategoryId} does not exist", request.CategoryId.Value);
                throw new InvalidOperationException($"Category with ID {request.CategoryId.Value} does not exist");
            }
        }

        var ambassadorUser = new User
        {
            Id = Guid.NewGuid(),
            Nickname = string.IsNullOrWhiteSpace(request.Nickname) ? request.Email.Split('@')[0] : request.Nickname,
            Name = request.Name,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Locality = request.Locality,
            IsWorking = request.IsWorking,
            About = request.About,
            LinkedIn = request.LinkedIn,
            Instagram = request.Instagram,
            Github = request.Github,
            IsActive = true
        };

        await _unitOfWork.Ambassadors.AddAsync(ambassadorUser, cancellationToken);
        await _unitOfWork.Ambassadors.EnsureAmbassadorRoleAsync(ambassadorUser, cancellationToken);

        if (request.CategoryId.HasValue)
        {
            await UpsertIntranetCategoryAsync(
                ambassadorUser.Id,
                request.CategoryId.Value,
                request.OtherCategory,
                cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador created successfully with user ID: {AmbassadorId}", ambassadorUser.Id);

        var createdAmbassador = await _unitOfWork.Ambassadors.GetAmbassadorWithDetailsAsync(ambassadorUser.Id, cancellationToken)
            ?? ambassadorUser;
        var responses = await MapUsersToResponsesAsync([createdAmbassador], cancellationToken);
        return responses.First();
    }

    public async Task<AmbassadorResponse?> UpdateAmbassadorAsync(
        Guid id,
        UpdateAmbassadorRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating ambassador with ID: {AmbassadorId}", id);

        var ambassador = await _unitOfWork.Ambassadors.GetAmbassadorWithDetailsAsync(id, cancellationToken);

        if (ambassador == null)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or inactive", id);
            return null;
        }

        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(request.CategoryId.Value, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category with ID {CategoryId} does not exist", request.CategoryId.Value);
                throw new InvalidOperationException($"Category with ID {request.CategoryId.Value} does not exist");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Nickname)) ambassador.Nickname = request.Nickname;
        if (!string.IsNullOrWhiteSpace(request.Name)) ambassador.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.LastName)) ambassador.LastName = request.LastName;
        if (!string.IsNullOrWhiteSpace(request.Email)) ambassador.Email = request.Email;
        if (request.Phone != null) ambassador.Phone = request.Phone;
        if (request.Locality != null) ambassador.Locality = request.Locality;
        if (request.IsWorking.HasValue) ambassador.IsWorking = request.IsWorking.Value;
        if (request.About != null) ambassador.About = request.About;
        if (request.LinkedIn != null) ambassador.LinkedIn = request.LinkedIn;
        if (request.Instagram != null) ambassador.Instagram = request.Instagram;
        if (request.Github != null) ambassador.Github = request.Github;

        await _unitOfWork.Ambassadors.EnsureAmbassadorRoleAsync(ambassador, cancellationToken);

        if (request.CategoryId.HasValue)
        {
            await UpsertIntranetCategoryAsync(
                ambassador.Id,
                request.CategoryId.Value,
                request.OtherCategory,
                cancellationToken);
        }

        ambassador.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Ambassadors.UpdateAsync(ambassador, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador updated successfully: {AmbassadorId}", id);

        var updatedAmbassador = await _unitOfWork.Ambassadors.GetAmbassadorWithDetailsAsync(id, cancellationToken)
            ?? ambassador;
        var responses = await MapUsersToResponsesAsync([updatedAmbassador], cancellationToken);
        return responses.First();
    }

    public async Task<bool> DeleteAmbassadorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting ambassador with ID: {AmbassadorId}", id);

        var isAmbassador = await _unitOfWork.Ambassadors.IsAmbassadorAsync(id, cancellationToken);

        if (!isAmbassador)
        {
            _logger.LogWarning("Ambassador with ID {AmbassadorId} not found or already inactive", id);
            return false;
        }

        await _unitOfWork.Ambassadors.RemoveAmbassadorRoleAsync(id, cancellationToken);

        var intranetCategories = await _unitOfWork.IntranetUserCategories
            .FindAsync(uc => uc.UserId == id && uc.Active, cancellationToken);

        foreach (var category in intranetCategories)
        {
            category.Active = false;
            category.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.IntranetUserCategories.UpdateAsync(category, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ambassador role removed successfully for user: {AmbassadorId}", id);

        return true;
    }

    private async Task<IEnumerable<AmbassadorResponse>> MapUsersToResponsesAsync(
        IEnumerable<User> users,
        CancellationToken cancellationToken)
    {
        var userList = users.ToList();
        if (userList.Count == 0)
        {
            return [];
        }

        var userIds = userList.Select(u => u.Id).ToArray();

        var activeCategories = (await _unitOfWork.IntranetUserCategories
            .FindAsync(uc => userIds.Contains(uc.UserId) && uc.Active, cancellationToken))
            .GroupBy(uc => uc.UserId)
            .ToDictionary(g => g.Key, g => g.First());

        var categoryNames = new Dictionary<int, string>();
        var categoryIds = activeCategories.Values.Select(c => c.CategoryId).Distinct().ToArray();
        if (categoryIds.Length > 0)
        {
            var allCategories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);
            categoryNames = allCategories
                .Where(c => categoryIds.Contains(c.Id))
                .ToDictionary(c => c.Id, c => c.Name);
        }

        return userList.Select(user =>
        {
            activeCategories.TryGetValue(user.Id, out var activeCategory);
            var categoryName = activeCategory != null && categoryNames.TryGetValue(activeCategory.CategoryId, out var name)
                ? name
                : null;

            return new AmbassadorResponse
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Locality = user.Locality,
                IsWorking = user.IsWorking,
                CategoryId = activeCategory?.CategoryId,
                CategoryName = categoryName,
                OtherCategory = activeCategory?.Description,
                About = user.About,
                Skill = null,
                LinkedIn = user.LinkedIn,
                Instagram = user.Instagram,
                Github = user.Github,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive
            };
        }).ToArray();
    }

    private async Task UpsertIntranetCategoryAsync(
        Guid userId,
        int categoryId,
        string? otherCategory,
        CancellationToken cancellationToken)
    {
        var selectedCategory = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Category with ID {categoryId} does not exist");

        var existingCategory = (await _unitOfWork.IntranetUserCategories
                .FindAsync(uc => uc.UserId == userId && uc.Active, cancellationToken))
            .FirstOrDefault();

        if (existingCategory == null)
        {
            await _unitOfWork.IntranetUserCategories.AddAsync(new IntranetUserCategory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                Category = selectedCategory.Name,
                Description = otherCategory,
                Active = true,
                IsActive = true
            }, cancellationToken);
            return;
        }

        existingCategory.CategoryId = categoryId;
        existingCategory.Category = selectedCategory.Name;
        existingCategory.Description = otherCategory;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.IntranetUserCategories.UpdateAsync(existingCategory, cancellationToken);
    }
}
