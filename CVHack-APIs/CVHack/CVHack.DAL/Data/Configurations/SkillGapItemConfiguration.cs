using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL
{
    public class SkillGapItemConfiguration : IEntityTypeConfiguration<SkillGapItem>
    {
        public void Configure(EntityTypeBuilder<SkillGapItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Analysis)
                   .WithMany(a => a.SkillGapItems)
                   .HasForeignKey(i => i.AnalysisId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
