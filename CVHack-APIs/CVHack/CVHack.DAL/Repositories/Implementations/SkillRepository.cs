using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Skill>> SearchAsync(string? search)
    {
        var query = _context.Skills.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s => s.Name.Contains(search));

        return await query.OrderBy(s => s.Name).AsNoTracking().ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var query = _context.Skills.Where(s => s.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(s => s.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
