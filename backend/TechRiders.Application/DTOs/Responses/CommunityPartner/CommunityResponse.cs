namespace TechRiders.Application.DTOs.Responses.CommunityPartner;

public sealed class CommunityResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Website { get; init; }
    public string? LogoUrl { get; init; }
    public string? LinkedIn { get; init; }
    public string? Instagram { get; init; }
    public string? X { get; init; }
    public string? YouTube { get; init; }
    public string? Github { get; init; }
}