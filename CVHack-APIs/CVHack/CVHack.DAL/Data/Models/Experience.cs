namespace CVHack.DAL;

public class Experience
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public UserProfile Profile { get; set; } = null!;

    public string CompanyName { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
