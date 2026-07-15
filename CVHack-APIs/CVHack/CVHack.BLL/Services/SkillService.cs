using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;

    public SkillService(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    private static SkillResponseDto MapToDto(Skill s) => new() { Id = s.Id, Name = s.Name };

    public async Task<Result<List<SkillResponseDto>>> GetAllAsync(string? search = null)
    {
        var skills = await _skillRepository.SearchAsync(search);
        return Result<List<SkillResponseDto>>.Success(
            skills.Select(MapToDto).ToList(),
            "Skills retrieved successfully.");
    }

    public async Task<Result<SkillResponseDto>> GetByIdAsync(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        if (skill == null)
            return Result<SkillResponseDto>.Failure("Skill not found.", statusCode: 404);

        return Result<SkillResponseDto>.Success(MapToDto(skill));
    }

    public async Task<Result<SkillResponseDto>> CreateAsync(CreateSkillDto dto)
    {
        if (await _skillRepository.ExistsByNameAsync(dto.Name))
            return Result<SkillResponseDto>.Failure("A skill with this name already exists.", statusCode: 400);

        var skill = new Skill { Name = dto.Name };
        _skillRepository.Insert(skill);
        await _skillRepository.SaveChangesAsync();

        return Result<SkillResponseDto>.Success(MapToDto(skill), "Skill created successfully.", 201);
    }

    public async Task<Result<SkillResponseDto>> UpdateAsync(int id, UpdateSkillDto dto)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        if (skill == null)
            return Result<SkillResponseDto>.Failure("Skill not found.", statusCode: 404);

        if (await _skillRepository.ExistsByNameAsync(dto.Name, excludeId: id))
            return Result<SkillResponseDto>.Failure("A skill with this name already exists.", statusCode: 400);

        skill.Name = dto.Name;
        _skillRepository.Update(skill);
        await _skillRepository.SaveChangesAsync();

        return Result<SkillResponseDto>.Success(MapToDto(skill), "Skill updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        if (skill == null)
            return Result<bool>.Failure("Skill not found.", statusCode: 404);

        _skillRepository.Delete(skill);
        await _skillRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Skill deleted successfully.");
    }
}
