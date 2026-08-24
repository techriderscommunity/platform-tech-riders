using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities;

namespace TechRiders.Infrastructure.Persistence.Configurations;

public sealed class KnowledgeArticleConfiguration : IEntityTypeConfiguration<KnowledgeArticle>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticle> builder)
    {
        builder.ToTable("KnowledgeArticles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(220).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.Property(x => x.ContentMd).IsRequired();
        builder.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorUserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class KnowledgeArticleCategoryConfiguration : IEntityTypeConfiguration<KnowledgeArticleCategory>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticleCategory> builder)
    {
        builder.ToTable("KnowledgeArticleCategories");
        builder.HasKey(x => new { x.KnowledgeArticleId, x.CategoryId });
        builder.HasOne(x => x.KnowledgeArticle).WithMany(x => x.Categories).HasForeignKey(x => x.KnowledgeArticleId);
        builder.HasOne(x => x.Category).WithMany(x => x.KnowledgeArticleCategories).HasForeignKey(x => x.CategoryId);
    }
}

public sealed class KnowledgeArticleSkillConfiguration : IEntityTypeConfiguration<KnowledgeArticleSkill>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticleSkill> builder)
    {
        builder.ToTable("KnowledgeArticleSkills");
        builder.HasKey(x => new { x.KnowledgeArticleId, x.SkillId });
        builder.HasOne(x => x.KnowledgeArticle).WithMany(x => x.Skills).HasForeignKey(x => x.KnowledgeArticleId);
        builder.HasOne(x => x.Skill).WithMany(x => x.KnowledgeArticleSkills).HasForeignKey(x => x.SkillId);
    }
}

public sealed class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder.ToTable("Communities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Website).HasMaxLength(512);
        builder.Property(x => x.LogoUrl).HasMaxLength(512);
        builder.Property(x => x.LinkedIn).HasMaxLength(512);
        builder.Property(x => x.Instagram).HasMaxLength(512);
        builder.HasOne(x => x.ContactUser).WithMany().HasForeignKey(x => x.ContactUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
{
    public void Configure(EntityTypeBuilder<CommunityMember> builder)
    {
        builder.ToTable("CommunityMembers");
        builder.HasKey(x => new { x.CommunityId, x.UserId });
        builder.Property(x => x.Role).HasMaxLength(120);
        builder.HasOne(x => x.Community).WithMany(x => x.Members).HasForeignKey(x => x.CommunityId);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CommunityCollaborationConfiguration : IEntityTypeConfiguration<CommunityCollaboration>
{
    public void Configure(EntityTypeBuilder<CommunityCollaboration> builder)
    {
        builder.ToTable("CommunityCollaborations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.HasOne(x => x.Community).WithMany(x => x.Collaborations).HasForeignKey(x => x.CommunityId);
        builder.HasOne(x => x.Event).WithMany(x => x.CommunityCollaborations).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Website).HasMaxLength(512);
        builder.Property(x => x.LinkedIn).HasMaxLength(512);
        builder.Property(x => x.LogoUrl).HasMaxLength(512);
        builder.HasOne(x => x.ContactUser).WithMany().HasForeignKey(x => x.ContactUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class JobOfferConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.ToTable("JobOffers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.Location).HasMaxLength(160);
        builder.Property(x => x.ContractType).HasMaxLength(80);
        builder.Property(x => x.Url).HasMaxLength(512);
        builder.HasOne(x => x.Company).WithMany(x => x.JobOffers).HasForeignKey(x => x.CompanyId);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }
}
