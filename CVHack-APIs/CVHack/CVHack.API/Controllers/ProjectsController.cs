using CVHack.BLL;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile/projects")]
[Authorize(Policy = "JobSeekerOnly")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectsController(
        IProjectService projectService,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator)
    {
        _projectService = projectService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>Get all projects for the logged-in user's profile.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMyProjects()
    {
        var result = await _projectService.GetMyProjectsAsync(GetUserId());
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get a single project by ID (must belong to logged-in user).</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _projectService.GetByIdAsync(GetUserId(), id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Create a new project for the logged-in user's profile.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(Result<ProjectResponseDto>.Failure(errors, "Validation failed.", 400));
        }

        var result = await _projectService.CreateAsync(GetUserId(), dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Update an existing project (must belong to logged-in user).</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(Result<ProjectResponseDto>.Failure(errors, "Validation failed.", 400));
        }

        var result = await _projectService.UpdateAsync(GetUserId(), id, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Delete a project (must belong to logged-in user).</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(GetUserId(), id);
        return StatusCode(result.StatusCode, result);
    }
}
