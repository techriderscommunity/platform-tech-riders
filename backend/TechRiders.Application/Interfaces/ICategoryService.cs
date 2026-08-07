using TechRiders.Application.DTOs.Requests.Category;
using TechRiders.Application.DTOs.Responses.Category;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de categorías
/// Define el contrato de operaciones de negocio para categorías
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Obtiene todas las categorías activas
    /// </summary>
    Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    Task<CategoryResponse?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene categorías principales (sin padre)
    /// </summary>
    Task<IEnumerable<CategoryResponse>> GetMainCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene subcategorías de una categoría padre
    /// </summary>
    Task<IEnumerable<CategoryResponse>> GetSubCategoriesAsync(int fatherId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una categoría (desactiva)
    /// </summary>
    Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
}
