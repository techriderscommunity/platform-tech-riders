using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Session;
using TechRiders.Application.DTOs.Responses.Sessions;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;

namespace TechRiders.Application.Services;

/// <summary>
/// Servicio de aplicación para gestión de sesiones
/// Implementa la lógica de negocio y orquesta operaciones del dominio
/// </summary>
public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SessionService> _logger;

    public SessionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SessionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<SessionResponse>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo todas las sesiones activas");

        var sessions = await _unitOfWork.Sessions.GetActiveSessionsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<SessionResponse?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo sesión con ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetSessionWithEventAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Sesión con ID {SessionId} no encontrada o inactiva", id);
            return null;
        }

        return _mapper.Map<SessionResponse>(session);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsByEventIdAsync(
        Guid eventId, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo sesiones del evento: {EventId}", eventId);

        var sessions = await _unitOfWork.Sessions.GetSessionsByEventIdAsync(eventId, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsBySpeakerAsync(
        string speaker, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo sesiones del ponente: {Speaker}", speaker);

        var sessions = await _unitOfWork.Sessions.GetSessionsBySpeakerAsync(speaker, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsByLevelAsync(
        string level, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo sesiones de nivel: {Level}", level);

        var sessions = await _unitOfWork.Sessions.GetSessionsByLevelAsync(level, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<SessionResponse> CreateSessionAsync(
        CreateSessionRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creando nueva sesión: {SessionTitle}", request.Title);

        // Validación de negocio: tiempos
        if (request.EndTime <= request.StartTime)
        {
            _logger.LogWarning("Intento de crear sesión con horarios inválidos");
            throw new InvalidOperationException("La hora de finalización debe ser posterior a la hora de inicio");
        }

        // Validación de negocio: evento existe
        var evento = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);
        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Intento de crear sesión para evento inexistente o inactivo: {EventId}", request.EventId);
            throw new InvalidOperationException($"El evento con ID {request.EventId} no existe o está inactivo");
        }

        // Validación de negocio: conflicto de horarios en la sala
        if (!string.IsNullOrEmpty(request.Room))
        {
            var hasConflict = await _unitOfWork.Sessions.HasTimeConflictAsync(
                request.EventId, 
                request.Room, 
                request.StartTime, 
                request.EndTime,
                null,
                cancellationToken);

            if (hasConflict)
            {
                _logger.LogWarning("Conflicto de horario detectado en sala {Room}", request.Room);
                throw new InvalidOperationException($"Ya existe una sesión en la sala '{request.Room}' en ese horario");
            }
        }

        var session = _mapper.Map<Session>(request);

        await _unitOfWork.Sessions.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sesión creada exitosamente con ID: {SessionId}", session.Id);

        return _mapper.Map<SessionResponse>(session);
    }

    public async Task<SessionResponse?> UpdateSessionAsync(
        Guid id, 
        UpdateSessionRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Actualizando sesión con ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetByIdAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Sesión con ID {SessionId} no encontrada o inactiva", id);
            return null;
        }

        // Validación de negocio para tiempos
        var newStartTime = request.StartTime ?? session.StartTime;
        var newEndTime = request.EndTime ?? session.EndTime;

        if (newEndTime <= newStartTime)
        {
            _logger.LogWarning("Intento de actualizar sesión con horarios inválidos");
            throw new InvalidOperationException("La hora de finalización debe ser posterior a la hora de inicio");
        }

        // Validación de conflicto de horarios si cambia la sala o los horarios
        var newRoom = request.Room ?? session.Room;
        if (!string.IsNullOrEmpty(newRoom))
        {
            var hasConflict = await _unitOfWork.Sessions.HasTimeConflictAsync(
                session.EventId,
                newRoom,
                newStartTime,
                newEndTime,
                id,
                cancellationToken);

            if (hasConflict)
            {
                _logger.LogWarning("Conflicto de horario detectado en sala {Room}", newRoom);
                throw new InvalidOperationException($"Ya existe una sesión en la sala '{newRoom}' en ese horario");
            }
        }

        _mapper.Map(request, session);

        await _unitOfWork.Sessions.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sesión actualizada exitosamente: {SessionId}", id);

        return _mapper.Map<SessionResponse>(session);
    }       

    public async Task<bool> DeleteSessionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Eliminando sesión con ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetByIdAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Sesión con ID {SessionId} no encontrada o ya inactiva", id);
            return false;
        }

        // Eliminación lógica
        session.IsActive = false;

        await _unitOfWork.Sessions.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sesión eliminada (lógicamente) exitosamente: {SessionId}", id);

        return true;
    }
}
