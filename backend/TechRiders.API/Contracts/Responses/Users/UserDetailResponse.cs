namespace TechRiders.Api.Contracts.Responses.Users;

public sealed class UserDetailResponse
{
    public required Guid Id { get; set; }
    public required string Nickname { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Locality { get; set; }
    public string? About { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public string? X { get; set; }
    public string? YouTube { get; set; }
    public string? Github { get; set; }
    public required IReadOnlyList<string> Roles { get; set; }
    public string? MembershipStatus { get; set; }
    public string? CurrentProfile { get; set; }
    public required IReadOnlyList<string> ActiveCapabilities { get; set; }
    public required IReadOnlyList<UserOrganizationSummary> Organizations { get; set; }
    public required IReadOnlyList<UserSkillSummary> Skills { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class UserOrganizationSummary
{
    public required Guid OrganizationId { get; set; }
    public required string OrganizationName { get; set; }
    public required string RelationType { get; set; }
    public required string Status { get; set; }
}

public sealed class UserSkillSummary
{
    public required Guid SkillId { get; set; }
    public required string SkillName { get; set; }
    public required string Level { get; set; }
}
