using TechRiders.Application.DTOs.Responses.Public;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Counts dinamicos reales (BD) reutilizados por las paginas publicas.
/// No contiene contenido estatico (textos/cards/links); eso vive en el frontend.
/// </summary>
public interface IPublicStatsService
{
    Task<HomeStatsResponse> GetHomeStatsAsync(CancellationToken cancellationToken = default);
    Task<CentersStatsResponse> GetCentersStatsAsync(CancellationToken cancellationToken = default);
    Task<WomanTechStatsResponse> GetWomanTechStatsAsync(CancellationToken cancellationToken = default);
    Task<JoinStatsResponse> GetJoinStatsAsync(CancellationToken cancellationToken = default);
    Task<OrientaTechStatsResponse> GetOrientaTechStatsAsync(CancellationToken cancellationToken = default);
    Task<AboutStatsResponse> GetAboutStatsAsync(CancellationToken cancellationToken = default);
}
