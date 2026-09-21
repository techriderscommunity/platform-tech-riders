namespace TechRiders.Api.Contracts.Responses.Visibility;

public sealed class FieldVisibilityResponse
{
    public required string FieldKey { get; set; }
    public required string Visibility { get; set; }
}
