namespace TechRiders.Application.DTOs.Responses.Public;

/// <summary>Miembro de equipo real, proyectado desde <c>User</c>.</summary>
public sealed class PublicTeamMemberResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public string? About { get; init; }
    public string? LinkedIn { get; init; }
    public string? Instagram { get; init; }
    public string? X { get; init; }
    public string? YouTube { get; init; }
    public string? Github { get; init; }
}

/// <summary>Zona de equipo (clave de agrupacion) con sus miembros reales.</summary>
public sealed class PublicTeamZoneResponse
{
    public required string Key { get; init; }
    public required IReadOnlyList<PublicTeamMemberResponse> Members { get; init; }
}
