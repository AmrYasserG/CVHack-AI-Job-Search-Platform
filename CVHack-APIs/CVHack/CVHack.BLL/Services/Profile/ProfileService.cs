using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<Result<ProfileResponseDto>> GetProfileAsync(string userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return Result<ProfileResponseDto>.Failure("Profile not found.", statusCode: 404);
        }

        return Result<ProfileResponseDto>.Success(MapToDto(profile));
    }

    public async Task<Result<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return Result<ProfileResponseDto>.Failure("Profile not found.", statusCode: 404);
        }

        profile.Headline = dto.Headline;
        profile.Summary = dto.Summary;
        profile.Country = dto.Country;
        profile.City = dto.City;
        profile.PhoneNumber = dto.PhoneNumber;
        profile.LinkedInUrl = dto.LinkedInUrl;
        profile.GitHubUrl = dto.GitHubUrl;
        profile.PortfolioUrl = dto.PortfolioUrl;
        profile.JobTitle = dto.JobTitle;
        profile.UpdatedAt = DateTime.UtcNow;

        await _profileRepository.SaveChangesAsync();

        return Result<ProfileResponseDto>.Success(MapToDto(profile), "Profile updated successfully.");
    }

    private static ProfileResponseDto MapToDto(UserProfile profile)
    {
        return new ProfileResponseDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Headline = profile.Headline,
            Summary = profile.Summary,
            Country = profile.Country,
            City = profile.City,
            PhoneNumber = profile.PhoneNumber,
            LinkedInUrl = profile.LinkedInUrl,
            GitHubUrl = profile.GitHubUrl,
            PortfolioUrl = profile.PortfolioUrl,
            JobTitle = profile.JobTitle,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,
            Experiences = profile.Experiences.Select(e => new ExperienceResponseDto
            {
                Id = e.Id,
                CompanyName = e.CompanyName,
                JobTitle = e.JobTitle,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            }).ToList(),
            Educations = profile.Educations.Select(ed => new EducationResponseDto
            {
                Id = ed.Id,
                University = ed.University,
                Degree = ed.Degree,
                StartYear = ed.StartYear,
                EndYear = ed.EndYear,
                Grade = ed.Grade
            }).ToList(),
            Certifications = profile.Certifications.Select(c => new CertificationResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Provider = c.Provider,
                CredentialUrl = c.CredentialUrl,
                CertifiedAt = c.CertifiedAt
            }).ToList()
        };
    }
}
