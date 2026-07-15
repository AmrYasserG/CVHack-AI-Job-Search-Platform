using CVHack.Common;

namespace CVHack.BLL;

public interface IProfileSkillService
{
    Task<Result<List<ProfileSkillResponseDto>>> GetMySkillsAsync(string userId);
    Task<Result<ProfileSkillResponseDto>> AddSkillAsync(string userId, int skillId);
    Task<Result<bool>> RemoveSkillAsync(string userId, int skillId);
}
