using CVHack.Common;

namespace CVHack.BLL.Services.Education;

public interface IEducationService
{
    Task<Result<List<EducationResponseDto>>> GetAllAsync(string userId);
    Task<Result<EducationResponseDto>> CreateAsync(string userId, EducationDto dto);
    Task<Result<EducationResponseDto>> UpdateAsync(string userId, int id, EducationDto dto);
    Task<Result<bool>> DeleteAsync(string userId, int id);
}
