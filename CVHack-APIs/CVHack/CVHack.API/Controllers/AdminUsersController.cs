using CVHack.BLL;
using CVHack.BLL.Services.AdminUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminUserService.GetAllUsersAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _adminUserService.GetUserByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("{id}/promote")]
        public async Task<IActionResult> Promote(string id)
        {
            var result = await _adminUserService.PromoteToAdminAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, UpdateUserStatusDto dto)
        {
            var result = await _adminUserService.UpdateStatusAsync(id, dto.Status);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}/plan")]
        public async Task<IActionResult> UpdatePlan(string id, UpdateUserPlanDto dto)
        {
            var result = await _adminUserService.UpdatePlanAsync(id, dto.Plan);
            return StatusCode(result.StatusCode, result);
        }
    }
}