using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProfileRepository _profileRepository;

    public ProjectService(
        IProjectRepository projectRepository,
        IProfileRepository profileRepository)
    {
        _projectRepository = projectRepository;
        _profileRepository = profileRepository;
    }

    private static ProjectResponseDto MapToDto(Project p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Description = p.Description,
        GithubUrl = p.GithubUrl
    };

    public async Task<Result<List<ProjectResponseDto>>> GetMyProjectsAsync(string userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<List<ProjectResponseDto>>.Failure("Profile not found.", statusCode: 404);

        var projects = await _projectRepository.GetByProfileIdAsync(profile.Id);
        return Result<List<ProjectResponseDto>>.Success(
            projects.Select(MapToDto).ToList(),
            "Projects retrieved successfully.");
    }

    public async Task<Result<ProjectResponseDto>> GetByIdAsync(string userId, int id)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<ProjectResponseDto>.Failure("Profile not found.", statusCode: 404);

        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            return Result<ProjectResponseDto>.Failure("Project not found.", statusCode: 404);

        if (project.ProfileId != profile.Id)
            return Result<ProjectResponseDto>.Failure("You are not authorized to access this project.", statusCode: 403);

        return Result<ProjectResponseDto>.Success(MapToDto(project));
    }

    public async Task<Result<ProjectResponseDto>> CreateAsync(string userId, CreateProjectDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<ProjectResponseDto>.Failure("Profile not found.", statusCode: 404);

        var project = new Project
        {
            ProfileId = profile.Id,
            Title = dto.Title,
            Description = dto.Description,
            GithubUrl = dto.GithubUrl
        };

        _projectRepository.Insert(project);
        await _projectRepository.SaveChangesAsync();

        return Result<ProjectResponseDto>.Success(MapToDto(project), "Project created successfully.", 201);
    }

    public async Task<Result<ProjectResponseDto>> UpdateAsync(string userId, int id, UpdateProjectDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<ProjectResponseDto>.Failure("Profile not found.", statusCode: 404);

        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            return Result<ProjectResponseDto>.Failure("Project not found.", statusCode: 404);

        if (project.ProfileId != profile.Id)
            return Result<ProjectResponseDto>.Failure("You are not authorized to modify this project.", statusCode: 403);

        project.Title = dto.Title;
        project.Description = dto.Description;
        project.GithubUrl = dto.GithubUrl;

        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();

        return Result<ProjectResponseDto>.Success(MapToDto(project), "Project updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(string userId, int id)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result<bool>.Failure("Profile not found.", statusCode: 404);

        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            return Result<bool>.Failure("Project not found.", statusCode: 404);

        if (project.ProfileId != profile.Id)
            return Result<bool>.Failure("You are not authorized to delete this project.", statusCode: 403);

        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Project deleted successfully.");
    }
}
