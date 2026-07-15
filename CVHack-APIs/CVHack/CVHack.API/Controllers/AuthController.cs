using CVHack.BLL.DTOs.Auth;
using CVHack.BLL.Services.Auth;
using CVHack.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterJobSeekerDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterJobSeekerDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register/jobseeker")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterJobSeeker([FromBody] RegisterJobSeekerDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<AuthResultDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var result = await _authService.RegisterJobSeekerAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var response = Result<AuthResultDto>.Failure(errors, "One or more validation errors occurred.", 400);
            return BadRequest(response);
        }

        var result = await _authService.LoginAsync(dto);
        return StatusCode(result.StatusCode, result);
    }
}
