using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.Url).HasMaxLength(512);
        builder.Property(x => x.Location).HasMaxLength(300);
        builder.HasOne(x => x.EventType).WithMany(x => x.Events).HasForeignKey(x => x.EventTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Center).WithMany(x => x.Events).HasForeignKey(x => x.CenterId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.HasOne(x => x.Event).WithMany(x => x.Sessions).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Center).WithMany(x => x.Sessions).HasForeignKey(x => x.CenterId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.ToTable("EventCategories");
        builder.HasKey(x => new { x.EventId, x.CategoryId });
        builder.HasOne(x => x.Event).WithMany(x => x.Categories).HasForeignKey(x => x.EventId);
        builder.HasOne(x => x.Category).WithMany(x => x.EventCategories).HasForeignKey(x => x.CategoryId);
    }
}

public sealed class SessionSpeakerConfiguration : IEntityTypeConfiguration<SessionSpeaker>
{
    public void Configure(EntityTypeBuilder<SessionSpeaker> builder)
    {
        builder.ToTable("SessionSpeakers");
        builder.HasKey(x => new { x.SessionId, x.UserId });
        builder.HasOne(x => x.Session).WithMany(x => x.Speakers).HasForeignKey(x => x.SessionId);
        builder.HasOne(x => x.User).WithMany(x => x.SpeakerSessions).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SessionCategoryConfiguration : IEntityTypeConfiguration<SessionCategory>
{
    public void Configure(EntityTypeBuilder<SessionCategory> builder)
    {
        builder.ToTable("SessionCategories");
        builder.HasKey(x => new { x.SessionId, x.CategoryId });
        builder.HasOne(x => x.Session).WithMany(x => x.Categories).HasForeignKey(x => x.SessionId);
        builder.HasOne(x => x.Category).WithMany(x => x.SessionCategories).HasForeignKey(x => x.CategoryId);
    }
}

public sealed class SessionSkillConfiguration : IEntityTypeConfiguration<SessionSkill>
{
    public void Configure(EntityTypeBuilder<SessionSkill> builder)
    {
        builder.ToTable("SessionSkills");
        builder.HasKey(x => new { x.SessionId, x.SkillId });
        builder.HasOne(x => x.Session).WithMany(x => x.Skills).HasForeignKey(x => x.SessionId);
        builder.HasOne(x => x.Skill).WithMany(x => x.SessionSkills).HasForeignKey(x => x.SkillId);
    }
}

public sealed class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder.ToTable("EventRegistrations");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.EventId, x.UserId }).IsUnique();
        builder.Property(x => x.RegistrationStatus).HasConversion<int>();
        builder.Property(x => x.Feedback).HasMaxLength(4000);
        builder.HasOne(x => x.Event).WithMany(x => x.Registrations).HasForeignKey(x => x.EventId);
        builder.HasOne(x => x.User).WithMany(x => x.EventRegistrations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SessionRegistrationConfiguration : IEntityTypeConfiguration<SessionRegistration>
{
    public void Configure(EntityTypeBuilder<SessionRegistration> builder)
    {
        builder.ToTable("SessionRegistrations");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.SessionId, x.UserId }).IsUnique();
        builder.Property(x => x.RegistrationStatus).HasConversion<int>();
        builder.Property(x => x.Feedback).HasMaxLength(4000);
        builder.HasOne(x => x.Session).WithMany(x => x.Registrations).HasForeignKey(x => x.SessionId);
        builder.HasOne(x => x.User).WithMany(x => x.SessionRegistrations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
