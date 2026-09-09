using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.CommunityPartner;

public sealed class CreateCommunityPartnerApplicationRequest
{
    [Required, StringLength(200)]
    public required string Name { get; init; }

    [Required, Url, StringLength(300)]
    public required string Website { get; init; }

    [Url, StringLength(300)]
    public string? LogoUrl { get; init; }

    [Required, EmailAddress, StringLength(200)]
    public required string ContactEmail { get; init; }

    [Required, StringLength(200)]
    public required string ContactName { get; init; }

    [Required, StringLength(2000)]
    public required string WhoYouAre { get; init; }

    [Required, StringLength(2000)]
    public required string WhatYouDo { get; init; }

    [Required, StringLength(2000)]
    public required string Mission { get; init; }

    [Required, StringLength(1000)]
    public required string Topics { get; init; }

    [Required, RegularExpression("local|national|international")]
    public required string Scope { get; init; }

    [RegularExpression(@"^[^:/?#\s]+$"), StringLength(300)]
    public string? LinkedIn { get; init; }

    [RegularExpression(@"^[^:/?#\s]+$"), StringLength(300)]
    public string? Instagram { get; init; }

    [RegularExpression(@"^[^:/?#\s]+$"), StringLength(300)]
    public string? X { get; init; }

    [RegularExpression(@"^[^:/?#\s]+$"), StringLength(300)]
    public string? YouTube { get; init; }

    [RegularExpression(@"^[^:/?#\s]+$"), StringLength(300)]
    public string? Github { get; init; }

    [Required, StringLength(2000)]
    public required string Motivation { get; init; }

    [Required, StringLength(2000)]
    public required string CollaborationIdeas { get; init; }
}