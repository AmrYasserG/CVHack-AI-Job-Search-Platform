using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class ProfileSkillService : IProfileSkillService
{
    private readonly IProfileSkillRepository _profileSkillRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IProfileRepository _profileRepository;

    public ProfileSkillService(
        IProfileSkillRepository profileSkillRepository,
        ISkillRepository skillRepository,
        IProfileRepository profileRepository)
    {
        _profileSkillRepository = profileSkillRepository;
        _skillRepository = skillRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Result<List<ProfileSkillResponseDto>>> GetMySkillsAsync(string userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<List<ProfileSkillResponseDto>>.Failure("Profile not found.", statusCode: 404);

        var profileSkills = await _profileSkillRepository.GetByProfileIdAsync(profile.Id);
        var dto = profileSkills.Select(ps => new ProfileSkillResponseDto
        {
            SkillId = ps.SkillId,
            SkillName = ps.Skill.Name
        }).ToList();

        return Result<List<ProfileSkillResponseDto>>.Success(dto, "Skills retrieved successfully.");
    }

    public async Task<Result<ProfileSkillResponseDto>> AddSkillAsync(string userId, int skillId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<ProfileSkillResponseDto>.Failure("Profile not found.", statusCode: 404);

        var skill = await _skillRepository.GetByIdAsync(skillId);
        if (skill == null)
            return Result<ProfileSkillResponseDto>.Failure("Skill not found.", statusCode: 404);

        if (await _profileSkillRepository.ExistsAsync(profile.Id, skillId))
            return Result<ProfileSkillResponseDto>.Failure("This skill is already added to your profile.", statusCode: 400);

        var profileSkill = new ProfileSkill { ProfileId = profile.Id, SkillId = skillId };
        _profileSkillRepository.Add(profileSkill);
        await _profileSkillRepository.SaveChangesAsync();

        return Result<ProfileSkillResponseDto>.Success(
            new ProfileSkillResponseDto { SkillId = skill.Id, SkillName = skill.Name },
            "Skill added to profile successfully.", 201);
    }

    public async Task<Result<bool>> RemoveSkillAsync(string userId, int skillId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<bool>.Failure("Profile not found.", statusCode: 404);

        var profileSkill = await _profileSkillRepository.GetAsync(profile.Id, skillId);
        if (profileSkill == null)
            return Result<bool>.Failure("Skill not found in your profile.", statusCode: 404);

        _profileSkillRepository.Remove(profileSkill);
        await _profileSkillRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Skill removed from profile successfully.");
    }
}
