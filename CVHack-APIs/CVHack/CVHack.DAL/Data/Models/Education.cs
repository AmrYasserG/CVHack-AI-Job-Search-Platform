namespace CVHack.DAL;

public class Education
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public UserProfile Profile { get; set; } = null!;

    public string University { get; set; } = null!;
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Grade { get; set; }
}
