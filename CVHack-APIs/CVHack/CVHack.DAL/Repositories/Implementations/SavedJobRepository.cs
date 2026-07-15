using Microsoft.EntityFrameworkCore;
namespace CVHack.DAL
{
    public class SavedJobRepository : GenericRepository<SavedJob>, ISavedJobRepository
    {
        public SavedJobRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SavedJob>> GetUserSavedJobsAsync(string userId)
        {
            return await _context.SavedJobs
                .Where(s => s.UserId == userId)
                .Include(s => s.Job)
                .OrderByDescending(s => s.SavedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsJobSavedAsync(string userId, int jobId)
        {
            return await _context.SavedJobs
                .AnyAsync(s => s.UserId == userId && s.JobId == jobId);
        }
    }
}