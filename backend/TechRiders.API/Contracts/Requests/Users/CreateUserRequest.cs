using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Users;

public sealed class CreateUserRequest
{
    [Required]
    public required string Nickname { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    public string? Phone { get; set; }
    public string? Locality { get; set; }
    public string? About { get; set; }
}
