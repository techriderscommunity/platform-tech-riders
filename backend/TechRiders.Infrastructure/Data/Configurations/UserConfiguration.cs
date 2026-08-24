using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nickname).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PasswordHash).HasMaxLength(512);
        builder.Property(x => x.PasswordResetToken).HasMaxLength(512);
        builder.Property(x => x.PasswordResetTokenExpiresAt);
        builder.Property(x => x.Phone).HasMaxLength(40);
        builder.Property(x => x.Locality).HasMaxLength(120);
        builder.Property(x => x.GPFId).HasMaxLength(80);
        builder.Property(x => x.LinkedIn).HasMaxLength(512);
        builder.Property(x => x.Instagram).HasMaxLength(512);
        builder.Property(x => x.Github).HasMaxLength(512);
        builder.Property(x => x.About).HasMaxLength(2000);
    }
}
