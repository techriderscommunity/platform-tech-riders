namespace TechRiders.Domain.Entities;

public sealed class Permission : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
