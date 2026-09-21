using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.TaxId).HasMaxLength(40);
        builder.Property(x => x.Website).HasMaxLength(300);
        builder.Property(x => x.Address).HasMaxLength(300);
        builder.Property(x => x.Province).HasMaxLength(120);
        builder.Property(x => x.Origin).HasMaxLength(120);
    }
}

public sealed class PersonOrganizationConfiguration : IEntityTypeConfiguration<PersonOrganization>
{
    public void Configure(EntityTypeBuilder<PersonOrganization> builder)
    {
        builder.ToTable("PersonOrganizations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Position).HasMaxLength(160);
        builder.Property(x => x.RelationType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.OrganizationRelations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Organization)
            .WithMany(x => x.PersonRelations)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class OrganizationGpfLinkConfiguration : IEntityTypeConfiguration<OrganizationGpfLink>
{
    public void Configure(EntityTypeBuilder<OrganizationGpfLink> builder)
    {
        builder.ToTable("OrganizationGpfLinks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.GpfReference).HasMaxLength(80);
        builder.Property(x => x.LinkMethod).HasMaxLength(80);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.Organization)
            .WithOne(x => x.GpfLink)
            .HasForeignKey<OrganizationGpfLink>(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.OrganizationId).IsUnique();
        // Regla de integridad: una referencia GPF no puede estar vinculada activamente a dos organizaciones.
        builder.HasIndex(x => x.GpfReference)
            .IsUnique()
            .HasFilter("[Status] = 1 AND [GpfReference] IS NOT NULL");
    }
}

public sealed class GpfPersonLinkConfiguration : IEntityTypeConfiguration<GpfPersonLink>
{
    public void Configure(EntityTypeBuilder<GpfPersonLink> builder)
    {
        builder.ToTable("GpfPersonLinks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CodUnico).HasMaxLength(80).IsRequired();
        builder.Property(x => x.LinkMethod).HasMaxLength(80);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.HasOne(x => x.User)
            .WithOne(x => x.GpfLink)
            .HasForeignKey<GpfPersonLink>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // Regla de integridad (Relaciones GPF §3): máximo un vínculo activo por persona y por CodUnico.
        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("[Status] = 1");
        builder.HasIndex(x => x.CodUnico)
            .IsUnique()
            .HasFilter("[Status] = 1");
    }
}
