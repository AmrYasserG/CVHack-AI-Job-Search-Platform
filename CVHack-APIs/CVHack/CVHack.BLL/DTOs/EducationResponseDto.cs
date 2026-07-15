namespace CVHack.BLL;

public class EducationResponseDto
{
    public int Id { get; set; }
    public string University { get; set; } = default!;
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Grade { get; set; }
}
