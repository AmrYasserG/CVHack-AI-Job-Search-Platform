using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Project>> GetByProfileIdAsync(int profileId)
        => await _context.Projects
            .Where(p => p.ProfileId == profileId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Project?> GetByIdAndProfileAsync(int id, int profileId)
        => await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.ProfileId == profileId);
}
