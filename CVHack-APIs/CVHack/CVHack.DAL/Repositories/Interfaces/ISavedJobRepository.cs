namespace CVHack.DAL
{
    public interface ISavedJobRepository : IGenericRepository<SavedJob>
    {
        Task<IEnumerable<SavedJob>> GetUserSavedJobsAsync(string userId);
        Task<bool> IsJobSavedAsync(string userId, int jobId);
    }
}