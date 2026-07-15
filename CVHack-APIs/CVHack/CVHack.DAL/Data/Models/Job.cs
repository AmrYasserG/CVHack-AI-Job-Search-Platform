namespace CVHack.DAL
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string SourcePlatform { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? BriefDescription { get; set; }
        public string City { get; set; } = null!;        
        public string Country { get; set; } = null!;      
        public string Seniority { get; set; } = null!;
        public string WorkType { get; set; } = null!;
        public string WorkTime { get; set; } = null!;
        public string JobUrl { get; set; } = null!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public DateTime PostedAt { get; set; }
        public string? ExternalId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime LastSeenAt { get; set; }
        public string? Responsibilities { get; set; }   // JSON array of strings (extracted from the posting)
        public string? Requirements { get; set; }        // JSON array of strings (extracted from the posting)
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new HashSet<SavedJob>();
        public ICollection<SkillGapAnalysis> SkillGapAnalyses { get; set; } = new HashSet<SkillGapAnalysis>();
    }
}
