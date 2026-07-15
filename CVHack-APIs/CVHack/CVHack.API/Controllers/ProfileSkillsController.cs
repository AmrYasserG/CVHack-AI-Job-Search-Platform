using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile/skills")]
[Authorize(Policy = "JobSeekerOnly")]
public class ProfileSkillsController : ControllerBase
{
    private readonly IProfileSkillService _profileSkillService;

    public ProfileSkillsController(IProfileSkillService profileSkillService)
    {
        _profileSkillService = profileSkillService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>Get all skills on the logged-in user's profile.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMySkills()
    {
        var result = await _profileSkillService.GetMySkillsAsync(GetUserId());
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Add a skill to the logged-in user's profile.</summary>
    [HttpPost("{skillId}")]
    public async Task<IActionResult> AddSkill(int skillId)
    {
        var result = await _profileSkillService.AddSkillAsync(GetUserId(), skillId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Remove a skill from the logged-in user's profile.</summary>
    [HttpDelete("{skillId}")]
    public async Task<IActionResult> RemoveSkill(int skillId)
    {
        var result = await _profileSkillService.RemoveSkillAsync(GetUserId(), skillId);
        return StatusCode(result.StatusCode, result);
    }
}
