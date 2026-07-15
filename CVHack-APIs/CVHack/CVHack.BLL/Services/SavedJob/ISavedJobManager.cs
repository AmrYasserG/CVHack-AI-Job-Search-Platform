using CVHack.Common;

namespace CVHack.BLL
{
    public interface ISavedJobManager
    {
        Task<Result<SavedJobReadDto>> SaveJobAsync(SavedJobCreateDto dto, string userId);
        Task<Result<IEnumerable<SavedJobReadDto>>> GetUserSavedJobsAsync(string userId);
        Task<Result<bool>> RemoveSavedJobAsync(int jobId, string userId);
    }
}