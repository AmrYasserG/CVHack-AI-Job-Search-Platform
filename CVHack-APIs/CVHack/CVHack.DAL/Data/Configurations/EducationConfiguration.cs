using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.University)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Degree)
               .HasMaxLength(150);

        builder.Property(e => e.Grade)
               .HasMaxLength(50);

        builder.HasOne(e => e.Profile)
               .WithMany(p => p.Educations)
               .HasForeignKey(e => e.ProfileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
