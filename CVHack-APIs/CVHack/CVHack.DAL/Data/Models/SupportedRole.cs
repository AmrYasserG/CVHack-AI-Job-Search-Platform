namespace CVHack.DAL;

public class SupportedRole
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? SearchQuery { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastIngestedAt { get; set; }
}
