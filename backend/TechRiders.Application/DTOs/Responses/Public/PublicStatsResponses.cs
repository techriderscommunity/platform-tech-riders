namespace TechRiders.Application.DTOs.Responses.Public;

/// <summary>Counts dinamicos reales para la home publica.</summary>
public sealed class HomeStatsResponse
{
    public required int ActiveAmbassadors { get; init; }
    public required int ActiveEvents { get; init; }
    public required int ActiveSessions { get; init; }
    public required int ActiveTrainingCenters { get; init; }
}

/// <summary>Count dinamico real de centros activos.</summary>
public sealed class CentersStatsResponse
{
    public required int ActiveTrainingCenters { get; init; }
}

/// <summary>Count dinamico real de ambassadors activos.</summary>
public sealed class WomanTechStatsResponse
{
    public required int ActiveAmbassadors { get; init; }
}

/// <summary>Count dinamico real de ambassadors activos (miembros).</summary>
public sealed class JoinStatsResponse
{
    public required int ActiveAmbassadors { get; init; }
}

/// <summary>Count dinamico real de sesiones activas.</summary>
public sealed class OrientaTechStatsResponse
{
    public required int ActiveSessions { get; init; }
}

/// <summary>Count dinamico real de ambassadors activos (comunidad).</summary>
public sealed class AboutStatsResponse
{
    public required int ActiveAmbassadors { get; init; }
}
