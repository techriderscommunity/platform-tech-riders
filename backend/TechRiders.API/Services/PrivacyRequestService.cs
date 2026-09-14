using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Solicitudes de ejercicio de derechos de privacidad (Requisitos Arquitectura §8.6). Plazos/procedimiento definitivos pendientes del DPO.</summary>
public static class PrivacyRequestService
{
    public static async Task<PrivacyRequest> CreateAsync(TechRidersDbContext dbContext, Guid userId, string requestType, string? channel, CancellationToken cancellationToken = default)
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

        await dbContext.Set<PrivacyRequest>().AddAsync(request, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public static Task<List<PrivacyRequest>> GetMineAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<PrivacyRequest>().Where(r => r.UserId == userId).OrderByDescending(r => r.RequestedAt).ToListAsync(cancellationToken);

    public static Task<List<PrivacyRequest>> ListAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<PrivacyRequest>().Include(r => r.User).OrderByDescending(r => r.RequestedAt).ToListAsync(cancellationToken);

    public static async Task<PrivacyRequest> ResolveAsync(TechRidersDbContext dbContext, Guid requestId, Guid responsibleUserId, string status, string? resolution, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PrivacyRequestStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            throw new ArgumentException($"Estado '{status}' no reconocido.");
        }

        var request = await dbContext.Set<PrivacyRequest>().FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        request.Status = parsedStatus;
        request.ResponsibleUserId = responsibleUserId;
        request.Resolution = resolution;
        if (parsedStatus is PrivacyRequestStatus.Resuelta or PrivacyRequestStatus.RechazadaConJustificacion or PrivacyRequestStatus.Cancelada)
        {
            request.ResolvedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }
}
