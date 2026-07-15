using CVHack.BLL.Services.Education;
using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class EducationService : IEducationService
{
    private readonly IEducationRepository _educationRepository;
    private readonly IProfileRepository _profileRepository;

    public EducationService(
        IEducationRepository educationRepository,
        IProfileRepository profileRepository)
    {
        _educationRepository = educationRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Result<List<EducationResponseDto>>> GetAllAsync(string userId)
    {
        var educations = await _educationRepository.GetAllByUserIdAsync(userId);
        var dtos = educations.Select(MapToDto).ToList();

        return Result<List<EducationResponseDto>>.Success(dtos);
    }

    public async Task<Result<EducationResponseDto>> CreateAsync(string userId, EducationDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return Result<EducationResponseDto>.Failure("Profile not found.", statusCode: 404);
        }

        var education = new Education
        {
            ProfileId = profile.Id,
            University = dto.University,
            Degree = dto.Degree,
            StartYear = dto.StartYear,
            EndYear = dto.EndYear,
            Grade = dto.Grade
        };

        await _educationRepository.AddAsync(education);
        await _educationRepository.SaveChangesAsync();

        return Result<EducationResponseDto>.Success(MapToDto(education), "Education added successfully.", 201);
    }

    public async Task<Result<EducationResponseDto>> UpdateAsync(string userId, int id, EducationDto dto)
    {
        var education = await _educationRepository.GetByIdAsync(id);
        if (education == null)
        {
            return Result<EducationResponseDto>.Failure("Education not found.", statusCode: 404);
        }

        if (education.Profile.UserId != userId)
        {
            return Result<EducationResponseDto>.Failure("You do not have access to this education.", statusCode: 403);
        }

        education.University = dto.University;
        education.Degree = dto.Degree;
        education.StartYear = dto.StartYear;
        education.EndYear = dto.EndYear;
        education.Grade = dto.Grade;

        _educationRepository.Update(education);
        await _educationRepository.SaveChangesAsync();

        return Result<EducationResponseDto>.Success(MapToDto(education), "Education updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(string userId, int id)
    {
        var education = await _educationRepository.GetByIdAsync(id);
        if (education == null)
        {
            return Result<bool>.Failure("Education not found.", statusCode: 404);
        }

        if (education.Profile.UserId != userId)
        {
            return Result<bool>.Failure("You do not have access to this education.", statusCode: 403);
        }

        _educationRepository.Delete(education);
        await _educationRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Education deleted successfully.");
    }

    private static EducationResponseDto MapToDto(Education education)
    {
        return new EducationResponseDto
        {
            Id = education.Id,
            University = education.University,
            Degree = education.Degree,
            StartYear = education.StartYear,
            EndYear = education.EndYear,
            Grade = education.Grade
        };
    }
}
