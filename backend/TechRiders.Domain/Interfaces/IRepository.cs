using System.Linq.Expressions;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz genérica para operaciones CRUD básicas en el repositorio
/// Implementa el principio de Interface Segregation y Dependency Inversion
/// </summary>
/// <typeparam name="T">Tipo de entidad del dominio</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene entidades que cumplan una condición específica
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega múltiples entidades
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una entidad
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una entidad que cumpla una condición
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cuenta las entidades que cumplen una condición
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}
