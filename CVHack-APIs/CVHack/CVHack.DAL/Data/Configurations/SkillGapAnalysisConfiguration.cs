using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL
{
    public class SkillGapAnalysisConfiguration : IEntityTypeConfiguration<SkillGapAnalysis>
    {
        public void Configure(EntityTypeBuilder<SkillGapAnalysis> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.User)
                   .WithMany(u => u.SkillGapAnalyses)
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Job)
                   .WithMany(j => j.SkillGapAnalyses)
                   .HasForeignKey(s => s.JobId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
