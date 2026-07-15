using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class CertificationRepository : ICertificationRepository
{
    private readonly AppDbContext _dbContext;

    public CertificationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Certification>> GetAllByUserIdAsync(string userId)
    {
        return await _dbContext.Certifications
            .Where(c => c.Profile.UserId == userId)
            .OrderByDescending(c => c.CertifiedAt)
            .ToListAsync();
    }

    public async Task<Certification?> GetByIdAsync(int id)
    {
        return await _dbContext.Certifications
            .Include(c => c.Profile)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Certification certification)
    {
        await _dbContext.Certifications.AddAsync(certification);
    }

    public void Update(Certification certification)
    {
        _dbContext.Certifications.Update(certification);
    }

    public void Delete(Certification certification)
    {
        _dbContext.Certifications.Remove(certification);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
