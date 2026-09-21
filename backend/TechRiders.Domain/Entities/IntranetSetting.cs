namespace TechRiders.Domain.Entities;

public sealed class IntranetSetting : BaseEntity
{
    public required string Key { get; set; }

    public required string Module { get; set; }

    public required string Value { get; set; }

    public required string Status { get; set; }

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public string UpdatedBy { get; set; } = string.Empty;

    public void Update(string module, string value, string status, string? updatedBy)
    {
        Module = module;
        Value = value;
        Status = status;
        UpdatedBy = updatedBy ?? string.Empty;
        UpdatedUtc = DateTime.UtcNow;
    }
}