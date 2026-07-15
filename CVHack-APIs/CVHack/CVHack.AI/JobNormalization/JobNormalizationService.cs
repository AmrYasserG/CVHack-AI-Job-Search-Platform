using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace CVHack.AI;

public class JobNormalizationService : IJobNormalizationService
{
    private readonly IChatClient _chat;
    private readonly ILogger<JobNormalizationService> _logger;

    private static readonly string[] AllowedSeniority = ["Junior", "Mid", "Senior"];
    private static readonly string[] AllowedWorkType  = ["Remote", "Hybrid", "On-site"];
    private static readonly string[] AllowedWorkTime  = ["Full-time", "Part-time", "Contract"];

    public JobNormalizationService(IChatClient chat, ILogger<JobNormalizationService> logger)
    {
        _chat = chat;
        _logger = logger;
    }

    public async Task<NormalizedJob?> NormalizeAsync(JoobleJob raw, CancellationToken ct = default)
    {
        var prompt = $"""
            Normalize this job posting into our schema. Use ONLY the provided text. Return JSON only.

            Fields and ALLOWED VALUES (copy exactly, character-for-character):
              city             — city name from location, else ""
              country          — full common English name (e.g. "Egypt", "United States"), else ""
              seniority        — exactly one of: "Junior" | "Mid" | "Senior"
              workType         — exactly one of: "Remote" | "Hybrid" | "On-site"
              workTime         — exactly one of: "Full-time" | "Part-time" | "Contract"
              salaryMin        — integer, else 0
              salaryMax        — integer >= salaryMin, else 0
              briefDescription — 2-3 factual sentences: what the role is + top requirements
              responsibilities — array of 3-6 short bullet strings describing what the person will do
              requirements     — array of 3-6 short bullet strings describing what's expected of the candidate

            Rules:
            - Values MUST be exactly from the allowed lists. Never paraphrase.
            - seniority: infer from years/title cues. No clear signal → "Mid".
            - workType: "Remote" only if explicit; "Hybrid" if both; else "On-site".
            - salary: only from clearly stated numbers; if ambiguous → 0 for both.
            - responsibilities/requirements: extract ONLY from the snippet. Rephrase into clean,
              concise bullets. Do NOT invent items not implied by the text. If the snippet has none,
              return an empty array. No HTML, no "..." fragments.

            JOB:
            Title: {raw.Title}
            Company: {raw.Company}
            Location: {raw.Location}
            Type: {raw.Type}
            Snippet: {raw.Snippet}
            """;

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                var ai = (await _chat.GetResponseAsync<NormalizedJobAi>(
                    prompt, useJsonSchemaResponseFormat: false, cancellationToken: ct)).Result;

                if (ai is null) return null;

                // Clamp values & handle rules
                int min = Math.Max(0, ai.SalaryMin);
                int max = Math.Max(0, ai.SalaryMax);
                if (max < min) max = min;

                return new NormalizedJob(
                    City:             ai.City ?? "",
                    Country:          ai.Country ?? "",
                    Seniority:        Allowed(ai.Seniority, AllowedSeniority, "Mid"),
                    WorkType:         Allowed(ai.WorkType, AllowedWorkType, "On-site"),
                    WorkTime:         Allowed(ai.WorkTime, AllowedWorkTime, "Full-time"),
                    SalaryMin:        min,
                    SalaryMax:        max,
                    BriefDescription: ai.BriefDescription ?? "",
                    Responsibilities: ai.Responsibilities ?? [],
                    Requirements:     ai.Requirements ?? []);
            }
            catch (Exception ex) when (ex.Message.Contains("429") || ex.Message.Contains("rate"))
            {
                _logger.LogWarning("Groq rate-limited (attempt {A}/3). Waiting {S}s...", attempt, 5 * attempt);
                await Task.Delay(TimeSpan.FromSeconds(5 * attempt), ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Normalization failed for '{T}'. Skipping.", raw.Title);
                return null;
            }
        }

        return null;
    }

    private static string Allowed(string? value, string[] allowed, string fallback)
        => allowed.FirstOrDefault(a => a.Equals(value, StringComparison.OrdinalIgnoreCase)) ?? fallback;

    private record NormalizedJobAi(
        string? City, string? Country,
        string? Seniority, string? WorkType, string? WorkTime,
        int SalaryMin, int SalaryMax,
        string? BriefDescription,
        string[]? Responsibilities, string[]? Requirements);
}
