using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "JobSeekerOnly")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;

        public ApplicationsController(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationCreateDto dto)
        {
            var userId = User.GetUserId();
            var result = await _applicationManager.CreateAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = User.GetUserId();
            var result = await _applicationManager.GetUserApplicationsAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}