using CVHack.Common;

namespace CVHack.BLL.Services.Experience;

public interface IExperienceService
{
    Task<Result<List<ExperienceResponseDto>>> GetAllAsync(string userId);
    Task<Result<ExperienceResponseDto>> CreateAsync(string userId, ExperienceDto dto);
    Task<Result<ExperienceResponseDto>> UpdateAsync(string userId, int id, ExperienceDto dto);
    Task<Result<bool>> DeleteAsync(string userId, int id);
}
