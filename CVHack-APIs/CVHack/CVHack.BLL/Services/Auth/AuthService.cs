using CVHack.BLL.DTOs.Auth;
using CVHack.Common;
using CVHack.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CVHack.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        AppDbContext dbContext)
    {
        _userManager = userManager;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<Result<AuthResultDto>> RegisterJobSeekerAsync(RegisterJobSeekerDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return Result<AuthResultDto>.Failure("Email is already in use.", statusCode: 400);
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description).ToList();
            return Result<AuthResultDto>.Failure(errors, "User creation failed.", statusCode: 400);
        }

        // Assign JobSeeker role
        var roleResult = await _userManager.AddToRoleAsync(user, "JobSeeker");
        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return Result<AuthResultDto>.Failure(errors, "Failed to assign role.", statusCode: 500);
        }

        // Create empty UserProfile
        var profile = new UserProfile
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.UserProfiles.Add(profile);
        await _dbContext.SaveChangesAsync();

        var authResult = await GenerateJwtTokenAsync(user);
        return Result<AuthResultDto>.Success(authResult, "JobSeeker registered successfully.", statusCode: 201);
    }

    public async Task<Result<AuthResultDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Result<AuthResultDto>.Failure("Invalid email or password.", statusCode: 401);
        }

        // block suspended accounts from logging in
        if (user.Status == "Suspended")
        {
            return Result<AuthResultDto>.Failure("Your account has been suspended. Please contact support.", statusCode: 403);
        }

        var authResult = await GenerateJwtTokenAsync(user);
        return Result<AuthResultDto>.Success(authResult, "Login successful.", statusCode: 200);
    }

    private async Task<AuthResultDto> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var durationInMinutes = double.Parse(jwtSettings["DurationInMinutes"] ?? "60");
        var expiration = DateTime.UtcNow.AddMinutes(durationInMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            expires: expiration,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new AuthResultDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
    }
}
