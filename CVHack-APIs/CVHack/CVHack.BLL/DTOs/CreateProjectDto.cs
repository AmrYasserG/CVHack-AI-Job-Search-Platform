namespace CVHack.BLL;

public class CreateProjectDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? GithubUrl { get; set; }
}
