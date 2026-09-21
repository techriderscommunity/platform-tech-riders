namespace TechRiders.Api.Contracts.Responses.Preferences;

public sealed class PreferenceDimensionValueResponse
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public Guid? ParentValueId { get; set; }
}

public sealed class PreferenceDimensionResponse
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required List<PreferenceDimensionValueResponse> Values { get; set; }
}
