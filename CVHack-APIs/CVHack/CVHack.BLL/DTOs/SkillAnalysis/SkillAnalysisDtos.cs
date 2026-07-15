namespace CVHack.BLL;

// API response
public class SkillAnalysisDto
{
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public int OverallScore { get; set; }
    public int AverageMatch { get; set; }   // average of the per-skill MatchPercent values (shown as the match score on the details page)
    public string OverallSummary { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public List<SkillAssessmentDto> Items { get; set; } = new();
}

public class SkillAssessmentDto
{
    public string SkillName { get; set; } = string.Empty;
    public int MatchPercent { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string WhyItMatters { get; set; } = string.Empty;
    public string SuggestedStep { get; set; } = string.Empty;
}

// what the LLM returns
public class SkillAnalysisAi
{
    public int OverallScore { get; set; }
    public string OverallSummary { get; set; } = string.Empty;
    public List<SkillAssessmentAi> Items { get; set; } = new();
}

public class SkillAssessmentAi
{
    public string SkillName { get; set; } = string.Empty;
    public int MatchPercent { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string WhyItMatters { get; set; } = string.Empty;
    public string SuggestedStep { get; set; } = string.Empty;
}