using MapsterMapper;
using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Event;
using TechRiders.Application.DTOs.Responses.Event;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Orchestrates event management while keeping persistence and DTO mapping outside the workflow.
/// </summary>
public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EventService> _logger;

    public EventService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EventService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all active events");

        var events = await _unitOfWork.Events.GetActiveEventsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<EventResponse?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting event with ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetEventWithSessionsAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Event with ID {EventId} was not found or is inactive", id);
            return null;
        }

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<IEnumerable<EventResponse>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting upcoming events");

        var events = await _unitOfWork.Events.GetUpcomingEventsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<IEnumerable<EventResponse>> SearchEventsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching events with term: {SearchTerm}", searchTerm);

        var events = await _unitOfWork.Events.SearchEventsAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<IEnumerable<EventResponse>> GetEventsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting events between {StartDate} and {EndDate}", startDate, endDate);

        var events = await _unitOfWork.Events.GetEventsByDateRangeAsync(startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<EventResponse> CreateEventAsync(
        CreateEventRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new event: {EventName}", request.Name);

        if (request.EndDate <= request.StartDate)
        {
            _logger.LogWarning("Attempt to create event with invalid dates");
            throw new InvalidOperationException("The end date must be later than the start date");
        }

        var evento = _mapper.Map<Event>(request);

        await _unitOfWork.Events.AddAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Event created successfully with ID: {EventId}", evento.Id);

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<EventResponse?> UpdateEventAsync(
        Guid id,
        UpdateEventRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating event with ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Event with ID {EventId} was not found or is inactive", id);
            return null;
        }

        var newStartDate = request.StartDate ?? evento.StartDate;
        var newEndDate = request.EndDate ?? evento.EndDate;

        if (newEndDate <= newStartDate)
        {
            _logger.LogWarning("Attempt to update event with invalid dates");
            throw new InvalidOperationException("The end date must be later than the start date");
        }

        _mapper.Map(request, evento);

        await _unitOfWork.Events.UpdateAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Event updated successfully: {EventId}", id);

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<bool> DeleteEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting event with ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Event with ID {EventId} was not found or is already inactive", id);
            return false;
        }

        evento.IsActive = false;

        await _unitOfWork.Events.UpdateAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Event logically deleted successfully: {EventId}", id);

        return true;
    }
}
