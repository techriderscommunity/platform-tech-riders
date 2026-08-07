using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Categorías
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    Task<MT_Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las categorías
    /// </summary>
    Task<IEnumerable<MT_Category>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene categorías activas
    /// </summary>
    Task<IEnumerable<MT_Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene categorías principales (sin padre)
    /// </summary>
    Task<IEnumerable<MT_Category>> GetMainCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene subcategorías de una categoría padre
    /// </summary>
    Task<IEnumerable<MT_Category>> GetSubCategoriesAsync(int fatherId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una categoría con sus subcategorías
    /// </summary>
    Task<MT_Category?> GetCategoryWithSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva categoría
    /// </summary>
    Task<MT_Category> AddAsync(MT_Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    Task UpdateAsync(MT_Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina (desactiva) una categoría
    /// </summary>
    Task DeleteAsync(MT_Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una categoría
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
