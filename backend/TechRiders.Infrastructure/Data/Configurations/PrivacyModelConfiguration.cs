using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class ConsentPurposeConfiguration : IEntityTypeConfiguration<ConsentPurpose>
{
    public void Configure(EntityTypeBuilder<ConsentPurpose> builder)
    {
        builder.ToTable("ConsentPurposes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}

public sealed class LegalBasisConfiguration : IEntityTypeConfiguration<LegalBasis>
{
    public void Configure(EntityTypeBuilder<LegalBasis> builder)
    {
        builder.ToTable("LegalBases");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}

public sealed class LegalTextConfiguration : IEntityTypeConfiguration<LegalText>
{
    public void Configure(EntityTypeBuilder<LegalText> builder)
    {
        builder.ToTable("LegalTexts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TextType).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Version).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.Purpose)
            .WithMany()
            .HasForeignKey(x => x.PurposeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.TextType, x.Version }).IsUnique();
    }
}

public sealed class ConsentConfiguration : IEntityTypeConfiguration<Consent>
{
    public void Configure(EntityTypeBuilder<Consent> builder)
    {
        builder.ToTable("Consents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.TextVersion).HasMaxLength(40);
        builder.Property(x => x.Origin).HasMaxLength(120);
        builder.Property(x => x.Evidence).HasMaxLength(500);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasMaxLength(300);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Consents)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Purpose)
            .WithMany()
            .HasForeignKey(x => x.PurposeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.LegalBasisEntity)
            .WithMany()
            .HasForeignKey(x => x.LegalBasisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PrivacyRequestConfiguration : IEntityTypeConfiguration<PrivacyRequest>
{
    public void Configure(EntityTypeBuilder<PrivacyRequest> builder)
    {
        builder.ToTable("PrivacyRequests");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RequestType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.Channel).HasMaxLength(80);
        builder.Property(x => x.Resolution).HasMaxLength(1000);
        builder.Property(x => x.InternalNotes).HasMaxLength(1000);
        builder.HasOne(x => x.User)
            .WithMany(x => x.PrivacyRequests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class UserFieldVisibilityConfiguration : IEntityTypeConfiguration<UserFieldVisibility>
{
    public void Configure(EntityTypeBuilder<UserFieldVisibility> builder)
    {
        builder.ToTable("UserFieldVisibilities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FieldKey).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Visibility).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.FieldVisibilities)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.UserId, x.FieldKey }).IsUnique();
    }
}
