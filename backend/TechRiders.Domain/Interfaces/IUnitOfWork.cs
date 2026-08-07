namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz Unit of Work para coordinar transacciones entre múltiples repositorios
/// Implementa el patrón Unit of Work para garantizar consistencia transaccional
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositorio de Eventos
    /// </summary>
    IEventRepository Events { get; }

    /// <summary>
    /// Repositorio de Sesiones
    /// </summary>
    ISessionRepository Sessions { get; }

    /// <summary>
    /// Repositorio de Ambassadors
    /// </summary>
    IAmbassadorRepository Ambassadors { get; }

    /// <summary>
    /// Repositorio de Centros
    /// </summary>
    ICenterRepository Centers { get; }

    /// <summary>
    /// Repositorio de Tours FP
    /// </summary>
    IFPTourRepository FPTours { get; }

    /// <summary>
    /// Repositorio de Categorías
    /// </summary>
    ICategoryRepository Categories { get; }

    /// <summary>
    /// Repositorio de Ofertas de Empleo
    /// </summary>
    IOfertaRepository Ofertas { get; }

    /// <summary>
    /// Repositorio de Candidaturas
    /// </summary>
    ICandidaturaRepository Candidaturas { get; }

    /// <summary>
    /// Repositorio de Tutoriales
    /// </summary>
    ITutorialRepository Tutoriales { get; }

    /// <summary>
    /// Repositorio de Audit Logs de Intranet
    /// </summary>
    IIntranetAuditLogRepository IntranetAuditLogs { get; }

    /// <summary>
    /// Repositorio de Configuración de Intranet
    /// </summary>
    IIntranetSettingRepository IntranetSettings { get; }

    /// <summary>
    /// Repositorio de Categorías de Usuario de Intranet
    /// </summary>
    IIntranetUserCategoryRepository IntranetUserCategories { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en una única transacción
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia una transacción explícita
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revierte la transacción actual
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
