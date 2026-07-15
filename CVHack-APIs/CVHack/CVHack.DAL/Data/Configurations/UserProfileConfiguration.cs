using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
               .IsRequired();

        builder.Property(p => p.Headline)
               .HasMaxLength(220);

        builder.Property(p => p.Summary)
               .HasMaxLength(2600);

        builder.Property(p => p.Country)
               .HasMaxLength(100);

        builder.Property(p => p.City)
               .HasMaxLength(100);

        builder.Property(p => p.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(p => p.LinkedInUrl)
               .HasMaxLength(500);

        builder.Property(p => p.GitHubUrl)
               .HasMaxLength(500);

        builder.Property(p => p.PortfolioUrl)
               .HasMaxLength(500);

        builder.Property(p => p.JobTitle)
               .HasMaxLength(150);

        builder.Property(p => p.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        //one profile per user
        builder.HasIndex(p => p.UserId)
               .IsUnique();

    }
}
