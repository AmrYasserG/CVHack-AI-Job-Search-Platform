using CVHack.BLL.Services.Experience;
using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class ExperienceService : IExperienceService
{
    private readonly IExperienceRepository _experienceRepository;
    private readonly IProfileRepository _profileRepository;

    public ExperienceService(
        IExperienceRepository experienceRepository,
        IProfileRepository profileRepository)
    {
        _experienceRepository = experienceRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Result<List<ExperienceResponseDto>>> GetAllAsync(string userId)
    {
        var experiences = await _experienceRepository.GetAllByUserIdAsync(userId);
        var dtos = experiences.Select(MapToDto).ToList();

        return Result<List<ExperienceResponseDto>>.Success(dtos);
    }

    public async Task<Result<ExperienceResponseDto>> CreateAsync(string userId, ExperienceDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return Result<ExperienceResponseDto>.Failure("Profile not found.", statusCode: 404);
        }

        var experience = new Experience
        {
            ProfileId = profile.Id,
            CompanyName = dto.CompanyName,
            JobTitle = dto.JobTitle,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        await _experienceRepository.AddAsync(experience);
        await _experienceRepository.SaveChangesAsync();

        return Result<ExperienceResponseDto>.Success(MapToDto(experience), "Experience added successfully.", 201);
    }

    public async Task<Result<ExperienceResponseDto>> UpdateAsync(string userId, int id, ExperienceDto dto)
    {
        var experience = await _experienceRepository.GetByIdAsync(id);
        if (experience == null)
        {
            return Result<ExperienceResponseDto>.Failure("Experience not found.", statusCode: 404);
        }

        if (experience.Profile.UserId != userId)
        {
            return Result<ExperienceResponseDto>.Failure("You do not have access to this experience.", statusCode: 403);
        }

        experience.CompanyName = dto.CompanyName;
        experience.JobTitle = dto.JobTitle;
        experience.StartDate = dto.StartDate;
        experience.EndDate = dto.EndDate;

        _experienceRepository.Update(experience);
        await _experienceRepository.SaveChangesAsync();

        return Result<ExperienceResponseDto>.Success(MapToDto(experience), "Experience updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(string userId, int id)
    {
        var experience = await _experienceRepository.GetByIdAsync(id);
        if (experience == null)
        {
            return Result<bool>.Failure("Experience not found.", statusCode: 404);
        }

        if (experience.Profile.UserId != userId)
        {
            return Result<bool>.Failure("You do not have access to this experience.", statusCode: 403);
        }

        _experienceRepository.Delete(experience);
        await _experienceRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Experience deleted successfully.");
    }

    private static ExperienceResponseDto MapToDto(Experience experience)
    {
        return new ExperienceResponseDto
        {
            Id = experience.Id,
            CompanyName = experience.CompanyName,
            JobTitle = experience.JobTitle,
            StartDate = experience.StartDate,
            EndDate = experience.EndDate
        };
    }
}
