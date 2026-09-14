using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class FPTourConfiguration : IEntityTypeConfiguration<FPTour>
{
    public void Configure(EntityTypeBuilder<FPTour> builder)
    {
        builder.ToTable("FPTours");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.HasOne(x => x.Organization).WithMany(x => x.FPTours).HasForeignKey(x => x.OrganizationId);
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