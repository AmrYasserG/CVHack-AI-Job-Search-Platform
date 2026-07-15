using CVHack.Common;

namespace CVHack.BLL;

public interface ISkillService
{
    Task<Result<List<SkillResponseDto>>> GetAllAsync(string? search = null);
    Task<Result<SkillResponseDto>> GetByIdAsync(int id);
    Task<Result<SkillResponseDto>> CreateAsync(CreateSkillDto dto);
    Task<Result<SkillResponseDto>> UpdateAsync(int id, UpdateSkillDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
