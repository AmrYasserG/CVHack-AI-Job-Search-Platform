using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.SalaryMin)
                   .HasPrecision(18, 2);

            builder.Property(j => j.SalaryMax)
                   .HasPrecision(18, 2);

            builder.Property(j => j.ExternalId)
                   .HasMaxLength(500);

            builder.Property(j => j.IsActive)
                   .HasDefaultValue(true);

            builder.HasIndex(j => new { j.SourcePlatform, j.ExternalId })
                   .IsUnique()
                   .HasFilter("[ExternalId] IS NOT NULL");
        }
    }
}
