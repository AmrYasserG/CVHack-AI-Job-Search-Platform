using CVHack.BLL;
using CVHack.BLL.Services.Education;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile/education")]
[Authorize]
public class EducationController : ControllerBase
{
    private readonly IEducationService _educationService;
    private readonly IValidator<EducationDto> _educationValidator;

    public EducationController(
        IEducationService educationService,
        IValidator<EducationDto> educationValidator)
    {
        _educationService = educationService;
        _educationValidator = educationValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var result = await _educationService.GetAllAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EducationDto dto)
    {
        var validationResult = await _educationValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<EducationResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _educationService.CreateAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EducationDto dto)
    {
        var validationResult = await _educationValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<EducationResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _educationService.UpdateAsync(userId, id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var result = await _educationService.DeleteAsync(userId, id);
        return StatusCode(result.StatusCode, result);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
