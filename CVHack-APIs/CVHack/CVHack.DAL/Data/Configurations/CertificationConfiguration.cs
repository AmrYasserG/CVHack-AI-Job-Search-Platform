using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(c => c.Provider)
               .HasMaxLength(200);

        builder.Property(c => c.CredentialUrl)
               .HasMaxLength(500);

        builder.HasOne(c => c.Profile)
               .WithMany(p => p.Certifications)
               .HasForeignKey(c => c.ProfileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
