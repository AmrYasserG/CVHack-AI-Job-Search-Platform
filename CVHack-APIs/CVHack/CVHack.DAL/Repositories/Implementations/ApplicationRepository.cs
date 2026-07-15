using Microsoft.EntityFrameworkCore;

namespace CVHack.DAL
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Application>> GetUserApplicationsAsync(string userId)
        {
            return await _context.Applications
                .Where(a => a.UserId == userId)
                .Include(a => a.Job)
                .OrderByDescending(a => a.AppliedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> HasUserAppliedAsync(string userId, int jobId)
        {
            return await _context.Applications
                .AnyAsync(a => a.UserId == userId && a.JobId == jobId);
        }

        //public async Task<int> GetUserApplicationsCountAsync(string userId)
        //{
        //    return await _context.Applications
        //        .CountAsync(a => a.UserId == userId);
        //}
    }
}