using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.JobTitle)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(e => e.StartDate)
               .IsRequired();

        builder.Property(e => e.EndDate)
               .IsRequired(false);

        builder.HasOne(e => e.Profile)
               .WithMany(p => p.Experiences)
               .HasForeignKey(e => e.ProfileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
