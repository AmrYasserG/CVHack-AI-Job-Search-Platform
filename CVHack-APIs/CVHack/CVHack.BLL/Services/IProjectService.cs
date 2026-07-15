using CVHack.Common;

namespace CVHack.BLL;

public interface IProjectService
{
    Task<Result<List<ProjectResponseDto>>> GetMyProjectsAsync(string userId);
    Task<Result<ProjectResponseDto>> GetByIdAsync(string userId, int id);
    Task<Result<ProjectResponseDto>> CreateAsync(string userId, CreateProjectDto dto);
    Task<Result<ProjectResponseDto>> UpdateAsync(string userId, int id, UpdateProjectDto dto);
    Task<Result<bool>> DeleteAsync(string userId, int id);
}
