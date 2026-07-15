namespace CVHack.DAL
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IEnumerable<Application>> GetUserApplicationsAsync(string userId);
        Task<bool> HasUserAppliedAsync(string userId, int jobId);
       // Task<int> GetUserApplicationsCountAsync(string userId);

    }
}