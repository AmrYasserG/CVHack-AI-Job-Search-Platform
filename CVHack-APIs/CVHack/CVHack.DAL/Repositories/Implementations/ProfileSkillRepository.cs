using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class ProfileSkillRepository : IProfileSkillRepository
{
    private readonly AppDbContext _context;

    public ProfileSkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProfileSkill>> GetByProfileIdAsync(int profileId)
        => await _context.ProfileSkills
            .Where(ps => ps.ProfileId == profileId)
            .Include(ps => ps.Skill)
            .AsNoTracking()
            .ToListAsync();

    public async Task<ProfileSkill?> GetAsync(int profileId, int skillId)
        => await _context.ProfileSkills
            .FirstOrDefaultAsync(ps => ps.ProfileId == profileId && ps.SkillId == skillId);

    public async Task<bool> ExistsAsync(int profileId, int skillId)
        => await _context.ProfileSkills
            .AnyAsync(ps => ps.ProfileId == profileId && ps.SkillId == skillId);

    public void Add(ProfileSkill profileSkill)
        => _context.ProfileSkills.Add(profileSkill);

    public void Remove(ProfileSkill profileSkill)
        => _context.ProfileSkills.Remove(profileSkill);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
