using CVHack.Common;

namespace CVHack.BLL
{
    public interface IJobManager
    {
        Task<Result<IEnumerable<JobReadDto>>> GetAllJobsAsync();
        Task<Result<JobReadDto>> GetJobByIdAsync(int id);
    }
}