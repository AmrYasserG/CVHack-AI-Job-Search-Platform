namespace CVHack.BLL;

public class ProjectResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? GithubUrl { get; set; }
}
