namespace CVHack.BLL;

public class ExperienceResponseDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
