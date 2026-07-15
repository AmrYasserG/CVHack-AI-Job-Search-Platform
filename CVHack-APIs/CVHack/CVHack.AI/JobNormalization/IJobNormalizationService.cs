namespace CVHack.AI;

public interface IJobNormalizationService
{
    Task<NormalizedJob?> NormalizeAsync(JoobleJob raw, CancellationToken ct = default);
}

public record NormalizedJob(
    string City,
    string Country,
    string Seniority,        // "Junior" | "Mid" | "Senior"
    string WorkType,         // "Remote" | "Hybrid" | "On-site"
    string WorkTime,         // "Full-time" | "Part-time" | "Contract"
    int SalaryMin,
    int SalaryMax,
    string BriefDescription,
    IReadOnlyList<string> Responsibilities,   // extracted from the snippet
    IReadOnlyList<string> Requirements);      // extracted from the snippet
