using CVHack.BLL;
using CVHack.BLL.Services.Profile;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IValidator<UpdateProfileDto> _updateProfileValidator;

    public ProfileController(
        IProfileService profileService,
        IValidator<UpdateProfileDto> updateProfileValidator)
    {
        _profileService = profileService;
        _updateProfileValidator = updateProfileValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();
        var result = await _profileService.GetProfileAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var validationResult = await _updateProfileValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<ProfileResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _profileService.UpdateProfileAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
