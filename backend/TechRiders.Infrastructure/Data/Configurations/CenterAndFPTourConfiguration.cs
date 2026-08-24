using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class CenterConfiguration : IEntityTypeConfiguration<Center>
{
    public void Configure(EntityTypeBuilder<Center> builder)
    {
        builder.ToTable("Centers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(40);
        builder.Property(x => x.Locality).HasMaxLength(120);
        builder.Property(x => x.Location).HasMaxLength(300);
        builder.Property(x => x.ParkingInfo).HasMaxLength(1000);
        builder.Property(x => x.LinkedIn).HasMaxLength(512);
        builder.Property(x => x.Instagram).HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(2000);
    }
}

public sealed class CenterStudyConfiguration : IEntityTypeConfiguration<CenterStudy>
{
    public void Configure(EntityTypeBuilder<CenterStudy> builder)
    {
        builder.ToTable("CenterStudies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Specialty).HasMaxLength(160);
        builder.HasOne(x => x.Center).WithMany(x => x.Studies).HasForeignKey(x => x.CenterId);
    }
}

public sealed class CenterContactConfiguration : IEntityTypeConfiguration<CenterContact>
{
    public void Configure(EntityTypeBuilder<CenterContact> builder)
    {
        builder.ToTable("CenterContacts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(40);
        builder.Property(x => x.Role).HasMaxLength(120);
        builder.HasOne(x => x.Center).WithMany(x => x.Contacts).HasForeignKey(x => x.CenterId);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class FPTourConfiguration : IEntityTypeConfiguration<FPTour>
{
    public void Configure(EntityTypeBuilder<FPTour> builder)
    {
        builder.ToTable("FPTours");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.HasOne(x => x.Center).WithMany(x => x.FPTours).HasForeignKey(x => x.CenterId);
        builder.HasOne(x => x.Ambassador).WithMany().HasForeignKey(x => x.AmbassadorUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class FPTourTaskConfiguration : IEntityTypeConfiguration<FPTourTask>
{
    public void Configure(EntityTypeBuilder<FPTourTask> builder)
    {
        builder.ToTable("FPTourTasks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TaskType).HasConversion<int>();
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(2000);
        builder.HasOne(x => x.FPTour).WithMany(x => x.Tasks).HasForeignKey(x => x.FPTourId);
    }
}
