namespace CVHack.BLL;

public class CompanyBriefingDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int? Founded { get; set; }
    public int? StaffMin { get; set; }
    public int? StaffMax { get; set; }
    public List<string> Content { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
}