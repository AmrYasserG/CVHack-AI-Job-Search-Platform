namespace CVHack.BLL;

public class EducationDto
{
    public string University { get; set; } = default!;
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Grade { get; set; }
}
