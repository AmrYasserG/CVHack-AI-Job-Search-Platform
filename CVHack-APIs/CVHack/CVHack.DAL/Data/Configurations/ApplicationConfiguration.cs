using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.User)
                   .WithMany(u => u.Applications)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Job)
                   .WithMany(j => j.Applications)
                   .HasForeignKey(a => a.JobId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.MatchScore)
                   .HasPrecision(5, 2); 
        }
    }
}
