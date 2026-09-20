using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Visibilidad de campos del perfil (Requisitos Arquitectura §10). Por defecto Private hasta configuracion explicita.</summary>
public interface IProfileVisibilityService
{
    Task<List<UserFieldVisibility>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserFieldVisibility> SetAsync(Guid userId, string fieldKey, string visibility, CancellationToken cancellationToken = default);
}
