using MapsterMapper;
using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Session;
using TechRiders.Application.DTOs.Responses.Sessions;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Orchestrates session lifecycle and validates time and room constraints for the event domain.
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
        _logger.LogInformation("Getting all active sessions");

        var sessions = await _unitOfWork.Sessions.GetActiveSessionsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<SessionResponse?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting session with ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetSessionWithEventAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Session with ID {SessionId} was not found or is inactive", id);
            return null;
        }

        return _mapper.Map<SessionResponse>(session);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsByEventIdAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting sessions for event: {EventId}", eventId);

        var sessions = await _unitOfWork.Sessions.GetSessionsByEventIdAsync(eventId, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsBySpeakerAsync(
        string speaker,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting sessions for speaker: {Speaker}", speaker);

        var sessions = await _unitOfWork.Sessions.GetSessionsBySpeakerAsync(speaker, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionsByLevelAsync(
        string level,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting sessions for level: {Level}", level);

        var sessions = await _unitOfWork.Sessions.GetSessionsByLevelAsync(level, cancellationToken);
        return _mapper.Map<IEnumerable<SessionResponse>>(sessions);
    }

    public async Task<SessionResponse> CreateSessionAsync(
        CreateSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new session: {SessionTitle}", request.Title);

        if (request.EndTime <= request.StartTime)
        {
            _logger.LogWarning("Attempt to create session with invalid time range");
            throw new InvalidOperationException("The end time must be later than the start time");
        }

        var evento = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);
        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Attempt to create session for inactive or missing event: {EventId}", request.EventId);
            throw new InvalidOperationException($"The event with ID {request.EventId} does not exist or is inactive");
        }

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
                _logger.LogWarning("Time conflict detected in room {Room}", request.Room);
                throw new InvalidOperationException($"A session already exists in room '{request.Room}' at that time");
            }
        }

        var session = _mapper.Map<Session>(request);

        await _unitOfWork.Sessions.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Session created successfully with ID: {SessionId}", session.Id);

        return _mapper.Map<SessionResponse>(session);
    }

    public async Task<SessionResponse?> UpdateSessionAsync(
        Guid id,
        UpdateSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating session with ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetByIdAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Session with ID {SessionId} was not found or is inactive", id);
            return null;
        }

        var newStartTime = request.StartTime ?? session.StartTime;
        var newEndTime = request.EndTime ?? session.EndTime;

        if (newEndTime <= newStartTime)
        {
            _logger.LogWarning("Attempt to update session with invalid time range");
            throw new InvalidOperationException("The end time must be later than the start time");
        }

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
                _logger.LogWarning("Time conflict detected in room {Room}", newRoom);
                throw new InvalidOperationException($"A session already exists in room '{newRoom}' at that time");
            }
        }

        _mapper.Map(request, session);

        await _unitOfWork.Sessions.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Session updated successfully: {SessionId}", id);

        return _mapper.Map<SessionResponse>(session);
    }

    public async Task<bool> DeleteSessionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting session with ID: {SessionId}", id);

        var session = await _unitOfWork.Sessions.GetByIdAsync(id, cancellationToken);

        if (session == null || !session.IsActive)
        {
            _logger.LogWarning("Session with ID {SessionId} was not found or is already inactive", id);
            return false;
        }

        session.IsActive = false;

        await _unitOfWork.Sessions.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Session logically deleted successfully: {SessionId}", id);

        return true;
    }
}
