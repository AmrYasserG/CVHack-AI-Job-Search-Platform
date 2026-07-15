namespace CVHack.DAL
{
    public class SkillGapItem
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public string SkillName { get; set; } = null!;
        public int MatchPercent { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string WhyItMatters { get; set; } = string.Empty;
        public string SuggestedStep { get; set; } = string.Empty;
        public SkillGapAnalysis Analysis { get; set; } = null!;
    }
}