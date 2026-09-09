namespace TechRiders.Domain.Entities;

public sealed class CommunityPartnerApplication : BaseEntity
{
    public required string Name { get; set; }
    public required string Website { get; set; }
    public string? LogoUrl { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactName { get; set; }
    public required string WhoYouAre { get; set; }
    public required string WhatYouDo { get; set; }
    public required string Mission { get; set; }
    public required string Topics { get; set; }
    public required string Scope { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public string? X { get; set; }
    public string? YouTube { get; set; }
    public string? Github { get; set; }
    public required string Motivation { get; set; }
    public required string CollaborationIdeas { get; set; }
    public required string Status { get; set; }
}