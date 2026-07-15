using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "JobSeekerOnly")]
    public class SavedJobsController : ControllerBase
    {
        private readonly ISavedJobManager _savedJobManager;

        public SavedJobsController(ISavedJobManager savedJobManager)
        {
            _savedJobManager = savedJobManager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveJob(SavedJobCreateDto dto)
        {
            var userId = User.GetUserId();
            var result = await _savedJobManager.SaveJobAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMySavedJobs()
        {
            var userId = User.GetUserId();
            var result = await _savedJobManager.GetUserSavedJobsAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{jobId:int}")]
        public async Task<IActionResult> RemoveSavedJob(int jobId)
        {
            var userId = User.GetUserId();
            var result = await _savedJobManager.RemoveSavedJobAsync(jobId, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}