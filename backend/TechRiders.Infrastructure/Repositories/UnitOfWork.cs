using Microsoft.EntityFrameworkCore.Storage;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work
/// Coordina transacciones entre múltiples repositorios
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly TechRidersDbContext _context;
    private IDbContextTransaction? _transaction;
    
    // Repositorios existentes
    private IEventRepository? _events;
    private ISessionRepository? _sessions;
    private IAmbassadorRepository? _ambassadors;
    private ICenterRepository? _centers;
    private IFPTourRepository? _fpTours;
    private ICategoryRepository? _categories;
    private IOfertaRepository? _ofertas;
    private ICandidaturaRepository? _candidaturas;
    private ITutorialRepository? _tutoriales;
    private IIntranetAuditLogRepository? _intranetAuditLogs;
    private IIntranetSettingRepository? _intranetSettings;
    private IIntranetUserCategoryRepository? _intranetUserCategories;

    public UnitOfWork(TechRidersDbContext context)
    {
        _context = context;
    }

    public IEventRepository Events => _events ??= new EventRepository(_context);

    public ISessionRepository Sessions => _sessions ??= new SessionRepository(_context);

    public IAmbassadorRepository Ambassadors => _ambassadors ??= new AmbassadorRepository(_context);

    public ICenterRepository Centers => _centers ??= new CenterRepository(_context);

    public IFPTourRepository FPTours => _fpTours ??= new FPTourRepository(_context);

    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);

    public IOfertaRepository Ofertas => _ofertas ??= new OfertaRepository(_context);

    public ICandidaturaRepository Candidaturas => _candidaturas ??= new CandidaturaRepository(_context);

    public ITutorialRepository Tutoriales => _tutoriales ??= new TutorialRepository(_context);

    public IIntranetAuditLogRepository IntranetAuditLogs => _intranetAuditLogs ??= new IntranetAuditLogRepository(_context);

    public IIntranetSettingRepository IntranetSettings => _intranetSettings ??= new IntranetSettingRepository(_context);

    public IIntranetUserCategoryRepository IntranetUserCategories => _intranetUserCategories ??= new IntranetUserCategoryRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
