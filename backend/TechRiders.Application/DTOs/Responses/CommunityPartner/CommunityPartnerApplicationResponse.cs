namespace TechRiders.Application.DTOs.Responses.CommunityPartner;

public sealed record CommunityPartnerApplicationResponse(
	Guid Id,
	string Status,
	string? LinkedIn,
	string? Instagram,
	string? X,
	string? YouTube,
	string? Github);