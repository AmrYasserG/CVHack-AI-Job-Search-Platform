using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _dbContext;

    public ProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await _dbContext.UserProfiles
            .Include(p => p.Experiences)
            .Include(p => p.Educations)
            .Include(p => p.Certifications)
            .Include(p => p.Projects)
            .Include(p => p.ProfileSkills)
                .ThenInclude(ps => ps.Skill)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<bool> ExistsForUserAsync(string userId)
    {
        return await _dbContext.UserProfiles.AnyAsync(p => p.UserId == userId);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    public async Task<UserProfile?> GetFullProfileAsync(string userId)
    {
        return await _dbContext.UserProfiles
            .Include(p => p.User)
            .Include(p => p.Experiences)
            .Include(p => p.Educations)
            .Include(p => p.ProfileSkills).ThenInclude(ps => ps.Skill)
            .Include(p => p.Projects)
            .Include(p => p.Certifications)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
}
