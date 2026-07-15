namespace CVHack.DAL;

public class CompanyBriefing
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public int? Founded { get; set; }    
    public int? StaffMin { get; set; }     
    public int? StaffMax { get; set; }     
    public string Content { get; set; } = null!;   
    public DateTime UpdatedAt { get; set; }
}