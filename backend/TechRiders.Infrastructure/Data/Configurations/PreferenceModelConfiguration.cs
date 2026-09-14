using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class PreferenceDimensionConfiguration : IEntityTypeConfiguration<PreferenceDimension>
{
    public void Configure(EntityTypeBuilder<PreferenceDimension> builder)
    {
        builder.ToTable("PreferenceDimensions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}

public sealed class PreferenceDimensionValueConfiguration : IEntityTypeConfiguration<PreferenceDimensionValue>
{
    public void Configure(EntityTypeBuilder<PreferenceDimensionValue> builder)
    {
        builder.ToTable("PreferenceDimensionValues");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.HasOne(x => x.Dimension)
            .WithMany(x => x.Values)
            .HasForeignKey(x => x.DimensionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ParentValue)
            .WithMany(x => x.ChildValues)
            .HasForeignKey(x => x.ParentValueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.DimensionId, x.Code }).IsUnique();
    }
}

public sealed class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.ToTable("UserPreferences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.Origin).HasMaxLength(120);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Preferences)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.DimensionValue)
            .WithMany(x => x.UserPreferences)
            .HasForeignKey(x => x.DimensionValueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.UserId, x.DimensionValueId }).IsUnique();
    }
}

public sealed class ContentClassificationConfiguration : IEntityTypeConfiguration<ContentClassification>
{
    public void Configure(EntityTypeBuilder<ContentClassification> builder)
    {
        builder.ToTable("ContentClassifications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ContentKind).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.DimensionValue)
            .WithMany(x => x.ContentClassifications)
            .HasForeignKey(x => x.DimensionValueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ContentKind, x.ContentId, x.DimensionValueId }).IsUnique();
    }
}
