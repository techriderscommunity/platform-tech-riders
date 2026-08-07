using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.Event;
using TechRiders.Application.DTOs.Responses.Event;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;
namespace TechRiders.Application.Services;

/// <summary>
/// Servicio de aplicación para gestión de eventos
/// Implementa la lógica de negocio y orquesta operaciones del dominio
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
        _logger.LogInformation("Obteniendo todos los eventos activos");

        var events = await _unitOfWork.Events.GetActiveEventsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<EventResponse?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo evento con ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetEventWithSessionsAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Evento con ID {EventId} no encontrado o inactivo", id);
            return null;
        }

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<IEnumerable<EventResponse>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo eventos próximos");

        var events = await _unitOfWork.Events.GetUpcomingEventsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<IEnumerable<EventResponse>> SearchEventsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Buscando eventos con término: {SearchTerm}", searchTerm);

        var events = await _unitOfWork.Events.SearchEventsAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<IEnumerable<EventResponse>> GetEventsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo eventos entre {StartDate} y {EndDate}", startDate, endDate);

        var events = await _unitOfWork.Events.GetEventsByDateRangeAsync(startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<EventResponse> CreateEventAsync(
        CreateEventRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creando nuevo evento: {EventName}", request.Name);

        // Validación de negocio
        if (request.EndDate <= request.StartDate)
        {
            _logger.LogWarning("Intento de crear evento con fechas inválidas");
            throw new InvalidOperationException("La fecha de finalización debe ser posterior a la fecha de inicio");
        }

        var evento = _mapper.Map<Event>(request);

        await _unitOfWork.Events.AddAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Evento creado exitosamente con ID: {EventId}", evento.Id);

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<EventResponse?> UpdateEventAsync(
        Guid id,
        UpdateEventRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Actualizando evento con ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Evento con ID {EventId} no encontrado o inactivo", id);
            return null;
        }

        // Validación de negocio para fechas
        var newStartDate = request.StartDate ?? evento.StartDate;
        var newEndDate = request.EndDate ?? evento.EndDate;

        if (newEndDate <= newStartDate)
        {
            _logger.LogWarning("Intento de actualizar evento con fechas inválidas");
            throw new InvalidOperationException("La fecha de finalización debe ser posterior a la fecha de inicio");
        }

        _mapper.Map(request, evento);

        await _unitOfWork.Events.UpdateAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Evento actualizado exitosamente: {EventId}", id);

        return _mapper.Map<EventResponse>(evento);
    }

    public async Task<bool> DeleteEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Eliminando evento con ID: {EventId}", id);

        var evento = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

        if (evento == null || !evento.IsActive)
        {
            _logger.LogWarning("Evento con ID {EventId} no encontrado o ya inactivo", id);
            return false;
        }

        // Eliminación lógica
        evento.IsActive = false;

        await _unitOfWork.Events.UpdateAsync(evento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Evento eliminado (lógicamente) exitosamente: {EventId}", id);

        return true;
    }
}
