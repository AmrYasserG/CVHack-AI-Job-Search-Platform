using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL
{
    public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
    {
        public void Configure(EntityTypeBuilder<SavedJob> builder)
        {
            builder.HasKey(s => new
            {
                s.UserId,
                s.JobId
            });

            builder.HasOne(s => s.User)
                   .WithMany(u => u.SavedJobs)
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Job)
                   .WithMany(j => j.SavedJobs)
                   .HasForeignKey(s => s.JobId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
