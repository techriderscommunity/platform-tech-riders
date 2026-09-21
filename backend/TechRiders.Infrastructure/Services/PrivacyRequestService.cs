using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Solicitudes de ejercicio de derechos de privacidad (Requisitos Arquitectura §8.6). Plazos/procedimiento definitivos pendientes del DPO.</summary>
public sealed class PrivacyRequestService : IPrivacyRequestService
{
    private readonly TechRidersDbContext _dbContext;

    public PrivacyRequestService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PrivacyRequest> CreateAsync(Guid userId, string requestType, string? channel, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PrivacyRequestType>(requestType, ignoreCase: true, out var parsedType))
        {
            throw new ArgumentException($"Tipo de solicitud '{requestType}' no reconocido.");
        }

        var request = new PrivacyRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RequestType = parsedType,
            Channel = channel,
            Status = PrivacyRequestStatus.Recibida,
            RequestedAt = DateTime.UtcNow,
        };

        await _dbContext.Set<PrivacyRequest>().AddAsync(request, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public Task<List<PrivacyRequest>> GetMineAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<PrivacyRequest>().Where(r => r.UserId == userId).OrderByDescending(r => r.RequestedAt).ToListAsync(cancellationToken);

    public Task<List<PrivacyRequest>> ListAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<PrivacyRequest>().Include(r => r.User).OrderByDescending(r => r.RequestedAt).ToListAsync(cancellationToken);

    public async Task<PrivacyRequest> ResolveAsync(Guid requestId, Guid responsibleUserId, string status, string? resolution, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PrivacyRequestStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            throw new ArgumentException($"Estado '{status}' no reconocido.");
        }

        var request = await _dbContext.Set<PrivacyRequest>().FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        request.Status = parsedStatus;
        request.ResponsibleUserId = responsibleUserId;
        request.Resolution = resolution;
        if (parsedStatus is PrivacyRequestStatus.Resuelta or PrivacyRequestStatus.RechazadaConJustificacion or PrivacyRequestStatus.Cancelada)
        {
            request.ResolvedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }
}
