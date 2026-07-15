namespace CVHack.BLL
{
    public class JobReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;      
        public string Country { get; set; } = string.Empty;
        public string Seniority { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public string WorkTime { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? BriefDescription { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public DateTime PostedAt { get; set; }
        public string JobUrl { get; set; } = string.Empty;
        public string SourcePlatform { get; set; } = string.Empty;
        public List<string> Responsibilities { get; set; } = new();
        public List<string> Requirements { get; set; } = new();
    }
}