using TechRiders.Application.DTOs.Responses.Public;

namespace TechRiders.Application.Interfaces;

/// <summary>Equipo real de la comunidad, agrupado por Capability, para la pagina "Quienes somos".</summary>
public interface IPublicTeamService
{
    Task<IReadOnlyList<PublicTeamZoneResponse>> GetTeamZonesAsync(CancellationToken cancellationToken = default);
}
