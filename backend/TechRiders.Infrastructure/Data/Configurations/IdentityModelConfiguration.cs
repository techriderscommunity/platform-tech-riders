using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.Origin).HasMaxLength(120);
        builder.Property(x => x.Reason).HasMaxLength(500);
        builder.HasOne(x => x.User)
            .WithOne(x => x.Membership)
            .HasForeignKey<Membership>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // Regla de integridad: una única membresía por persona.
        builder.HasIndex(x => x.UserId).IsUnique();
    }
}

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(80).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

public sealed class UserProfileHistoryConfiguration : IEntityTypeConfiguration<UserProfileHistory>
{
    public void Configure(EntityTypeBuilder<UserProfileHistory> builder)
    {
        builder.ToTable("UserProfileHistories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Notes).HasMaxLength(500);
        builder.HasOne(x => x.User)
            .WithMany(x => x.ProfileHistories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Profile)
            .WithMany(x => x.UserProfileHistories)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
        // Regla de integridad: un único perfil vigente por persona.
        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("[IsCurrent] = 1");
    }
}

public sealed class CapabilityConfiguration : IEntityTypeConfiguration<Capability>
{
    public void Configure(EntityTypeBuilder<Capability> builder)
    {
        builder.ToTable("Capabilities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(80).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

public sealed class UserCapabilityConfiguration : IEntityTypeConfiguration<UserCapability>
{
    public void Configure(EntityTypeBuilder<UserCapability> builder)
    {
        builder.ToTable("UserCapabilities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.Capabilities)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Capability)
            .WithMany(x => x.UserCapabilities)
            .HasForeignKey(x => x.CapabilityId)
            .OnDelete(DeleteBehavior.Restrict);
        // Regla de integridad: una capacidad activa por persona y tipo (permite histórico de rechazadas/revocadas).
        builder.HasIndex(x => new { x.UserId, x.CapabilityId })
            .IsUnique()
            .HasFilter("[Status] = 2");
    }
}

public sealed class PersonStudyConfiguration : IEntityTypeConfiguration<PersonStudy>
{
    public void Configure(EntityTypeBuilder<PersonStudy> builder)
    {
        builder.ToTable("PersonStudies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.StudyType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Specialty).HasMaxLength(160);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Studies)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class UserOrientationScopeConfiguration : IEntityTypeConfiguration<UserOrientationScope>
{
    public void Configure(EntityTypeBuilder<UserOrientationScope> builder)
    {
        builder.ToTable("UserOrientationScopes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Scope).HasConversion<int>().IsRequired();
        builder.Property(x => x.OtherDetail).HasMaxLength(160);
        builder.HasOne(x => x.User)
            .WithMany(x => x.OrientationScopes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.UserId, x.Scope }).IsUnique();
    }
}
