using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(x => new { x.UserId, x.RoleId });
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
    }
}

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });
        builder.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId);
        builder.HasOne(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId);
    }
}

public sealed class UserCategoryConfiguration : IEntityTypeConfiguration<UserCategory>
{
    public void Configure(EntityTypeBuilder<UserCategory> builder)
    {
        builder.ToTable("UserCategories");
        builder.HasKey(x => new { x.UserId, x.CategoryId });
        builder.HasOne(x => x.User).WithMany(x => x.UserCategories).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Category).WithMany(x => x.UserCategories).HasForeignKey(x => x.CategoryId);
    }
}

public sealed class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("UserSkills");
        builder.HasKey(x => new { x.UserId, x.SkillId });
        builder.HasOne(x => x.User).WithMany(x => x.UserSkills).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Skill).WithMany(x => x.UserSkills).HasForeignKey(x => x.SkillId);
        builder.Property(x => x.Level).HasConversion<int>();
    }
}
