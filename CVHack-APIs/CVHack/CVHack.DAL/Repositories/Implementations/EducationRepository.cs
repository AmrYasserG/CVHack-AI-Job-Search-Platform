using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class EducationRepository : IEducationRepository
{
    private readonly AppDbContext _dbContext;

    public EducationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Education>> GetAllByUserIdAsync(string userId)
    {
        return await _dbContext.Educations
            .Where(e => e.Profile.UserId == userId)
            .OrderByDescending(e => e.StartYear)
            .ToListAsync();
    }

    public async Task<Education?> GetByIdAsync(int id)
    {
        return await _dbContext.Educations
            .Include(e => e.Profile)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task AddAsync(Education education)
    {
        await _dbContext.Educations.AddAsync(education);
    }
    public void Update(Education education)
    {
        _dbContext.Educations.Update(education);
    }

    public void Delete(Education education)
    {
        _dbContext.Educations.Remove(education);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

}

