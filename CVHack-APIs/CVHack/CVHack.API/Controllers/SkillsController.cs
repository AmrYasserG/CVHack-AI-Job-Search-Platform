using CVHack.BLL;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _skillService;
    private readonly IValidator<CreateSkillDto> _createValidator;
    private readonly IValidator<UpdateSkillDto> _updateValidator;

    public SkillsController(
        ISkillService skillService,
        IValidator<CreateSkillDto> createValidator,
        IValidator<UpdateSkillDto> updateValidator)
    {
        _skillService = skillService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>Get all skills. Supports optional ?search= query parameter.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _skillService.GetAllAsync(search);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get a skill by ID.</summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _skillService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Create a new skill. Admin only.</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(Result<SkillResponseDto>.Failure(errors, "Validation failed.", 400));
        }

        var result = await _skillService.CreateAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Update an existing skill. Admin only.</summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(Result<SkillResponseDto>.Failure(errors, "Validation failed.", 400));
        }

        var result = await _skillService.UpdateAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Delete a skill. Admin only.</summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _skillService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
