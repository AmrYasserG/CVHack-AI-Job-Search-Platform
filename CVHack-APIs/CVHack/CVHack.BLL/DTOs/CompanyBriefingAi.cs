namespace CVHack.BLL;

public class CompanyBriefingAi
{
    public int? Founded { get; set; }
    public int? StaffMin { get; set; }
    public int? StaffMax { get; set; }
    public List<string> Content { get; set; } = new();
}