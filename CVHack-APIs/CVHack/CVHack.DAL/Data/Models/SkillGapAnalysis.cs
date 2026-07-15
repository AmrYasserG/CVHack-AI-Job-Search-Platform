namespace CVHack.DAL
{
    public class SkillGapAnalysis
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int JobId { get; set; }
        public int OverallScore { get; set; }
        public string OverallSummary { get; set; } = string.Empty;
        public string ProfileHash { get; set; } = string.Empty;   // fingerprint of the profile this was generated from
        public DateTime UpdatedAt { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Job Job { get; set; } = null!;
        public ICollection<SkillGapItem> SkillGapItems { get; set; } = new HashSet<SkillGapItem>();
    }
}