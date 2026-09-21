namespace TechRiders.Api.Contracts.Responses.Users;

public sealed class UserListItemResponse
{
    public required Guid Id { get; set; }
    public required string Nickname { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required IReadOnlyList<string> Roles { get; set; }
    public string? MembershipStatus { get; set; }
    public bool IsWorking { get; set; }
    public DateTimeOffset? LastActivityDate { get; set; }
}

public sealed class UserListResponse
{
    public required IReadOnlyList<UserListItemResponse> Items { get; set; }
    public required int TotalCount { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}
