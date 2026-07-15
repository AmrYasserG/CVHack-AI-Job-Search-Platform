namespace CVHack.DAL;

public class Project
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public UserProfile Profile { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? GithubUrl { get; set; }
}
