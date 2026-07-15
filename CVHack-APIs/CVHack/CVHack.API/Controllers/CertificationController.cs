using CVHack.BLL;
using CVHack.BLL.Services.Certification;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVHack.API;

[ApiController]
[Route("api/profile/certifications")]
[Authorize]
public class CertificationController : ControllerBase
{
    private readonly ICertificationService _certificationService;
    private readonly IValidator<CertificationDto> _certificationValidator;

    public CertificationController(
        ICertificationService certificationService,
        IValidator<CertificationDto> certificationValidator)
    {
        _certificationService = certificationService;
        _certificationValidator = certificationValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var result = await _certificationService.GetAllAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CertificationDto dto)
    {
        var validationResult = await _certificationValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<CertificationResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _certificationService.CreateAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CertificationDto dto)
    {
        var validationResult = await _certificationValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<CertificationResponseDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var userId = GetUserId();
        var result = await _certificationService.UpdateAsync(userId, id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var result = await _certificationService.DeleteAsync(userId, id);
        return StatusCode(result.StatusCode, result);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
