using CVHack.BLL;
using CVHack.BLL.Services.Experience;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile/experience")]
[Authorize]
public class ExperienceController : ControllerBase
{
    private readonly IExperienceService _experienceService;
    private readonly IValidator<ExperienceDto> _experienceValidator;

    public ExperienceController(
        IExperienceService experienceService,
        IValidator<ExperienceDto> experienceValidator)
    {
        _experienceService = experienceService;
        _experienceValidator = experienceValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var result = await _experienceService.GetAllAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExperienceDto dto)
    {
        var validationResult = await _experienceValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<ExperienceResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _experienceService.CreateAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExperienceDto dto)
    {
        var validationResult = await _experienceValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<ExperienceResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _experienceService.UpdateAsync(userId, id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var result = await _experienceService.DeleteAsync(userId, id);
        return StatusCode(result.StatusCode, result);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
