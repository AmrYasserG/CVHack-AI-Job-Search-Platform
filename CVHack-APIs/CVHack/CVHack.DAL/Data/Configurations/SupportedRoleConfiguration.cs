using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHack.DAL;

public class SupportedRoleConfiguration : IEntityTypeConfiguration<SupportedRole>
{
    public void Configure(EntityTypeBuilder<SupportedRole> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.SearchQuery).HasMaxLength(200);
        builder.Property(r => r.IsActive).HasDefaultValue(true);
    }
}
