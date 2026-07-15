using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class ExperienceRepository : IExperienceRepository
{
    private readonly AppDbContext _dbContext;

    public ExperienceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Experience>> GetAllByUserIdAsync(string userId)
    {
        return await _dbContext.Experiences
            .Where(e => e.Profile.UserId == userId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Experience?> GetByIdAsync(int id)
    {
        return await _dbContext.Experiences
            .Include(e => e.Profile)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Experience experience)
    {
        await _dbContext.Experiences.AddAsync(experience);
    }

    public void Update(Experience experience)
    {
        _dbContext.Experiences.Update(experience);
    }

    public void Delete(Experience experience)
    {
        _dbContext.Experiences.Remove(experience);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
