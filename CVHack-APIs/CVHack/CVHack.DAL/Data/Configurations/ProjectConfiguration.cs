using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(2000);

        builder.Property(p => p.GithubUrl)
               .HasMaxLength(500);

        builder.HasOne(p => p.Profile)
               .WithMany(up => up.Projects)
               .HasForeignKey(p => p.ProfileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
