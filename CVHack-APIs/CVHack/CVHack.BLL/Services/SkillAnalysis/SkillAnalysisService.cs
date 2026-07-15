using CVHack.Common;
using CVHack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

namespace CVHack.BLL;

public class SkillAnalysisService : ISkillAnalysisService
{
    private readonly IUnitOfWork _uow;
    private readonly IProfileRepository _profiles;
    private readonly AppDbContext _db;
    private readonly IChatClient _chat;

    public SkillAnalysisService(IUnitOfWork uow, IProfileRepository profiles, AppDbContext db, IChatClient chat)
    {
        _uow = uow; _profiles = profiles; _db = db; _chat = chat;
    }

    public async Task<Result<SkillAnalysisDto>> AnalyzeAsync(int jobId, string userId)
    {
        var job = await _uow.JobRepository.GetByIdAsync(jobId);
        if (job is null)
            return Result<SkillAnalysisDto>.Failure("Job not found.", "No such job.", 404);

        // cache (per user+job)
        var analysis = await _db.SkillGapAnalyses
            .Include(a => a.SkillGapItems)
            .FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == jobId);

        // load the candidate profile (needed both to fingerprint and to generate)
        var profile = await _profiles.GetByUserIdAsync(userId);
        if (profile is null)
            return Result<SkillAnalysisDto>.Failure("Profile not found.", "Complete your profile first.", 400);

        // without skills there is nothing meaningful to match against — tell the user to add them first
        if (!profile.ProfileSkills.Any())
            return Result<SkillAnalysisDto>.Failure("No skills in profile.", "Please add skills to your profile first.", 400);

        var currentHash = ProfileFingerprint.Compute(profile);

        // cache is valid only if the profile hasn't changed since it was generated
        if (analysis is not null && analysis.ProfileHash == currentHash)
            return Result<SkillAnalysisDto>.Success(MapToDto(job, analysis));

        var skills = string.Join(", ", profile.ProfileSkills.Select(ps => ps.Skill.Name));
        var experience = string.Join("; ", profile.Experiences.Select(e =>
            $"{e.JobTitle} at {e.CompanyName} ({e.StartDate:yyyy}-{(e.EndDate.HasValue ? e.EndDate.Value.Year.ToString() : "Present")})"));
        var education = string.Join("; ", profile.Educations.Select(ed => $"{ed.Degree}, {ed.University}"));
        var certs = string.Join("; ", profile.Certifications.Select(c => c.Name));
        var projects = string.Join("; ", profile.Projects.Select(pr => $"{pr.Title}: {pr.Description}"));

        // total years of professional experience (summed across roles) — gives the model a clear number for seniority fit
        var totalYears = Math.Round(profile.Experiences.Sum(e =>
            ((e.EndDate ?? DateTime.UtcNow) - e.StartDate).TotalDays / 365.25), 1);

        // Fix 1: make emptiness explicit so the model doesn't fill the vacuum
        static string Or(string s, string fallback) => string.IsNullOrWhiteSpace(s) ? fallback : s;

        var prompt = $"""
            You are a career coach. Assess how well the CANDIDATE matches the JOB.

            1. ALWAYS make the FIRST requirement "Experience level" (category "experience"). Score it strictly by comparing the JOB's seniority to the candidate's TOTAL years of experience, using these bands:
               - Junior / Entry: ~0-2 years
               - Mid / Intermediate: ~3-5 years
               - Senior: ~6+ years
               If the candidate has FEWER years than the job's level expects, this is a CRITICAL gap and its matchPercent MUST be low and proportional to the shortfall (e.g. a Mid-level job with 1 year of experience ≈ 25-40%; a Senior job with 2 years ≈ 15-30%). Do NOT be generous here — under-seniority is a serious mismatch.
            2. Then identify 5-7 more key skills/requirements for this job (technical skills, qualifications).
            3. For EACH, give matchPercent (0-100) = how well the candidate's profile meets it. Include both strong matches and weak ones.
            4. For weak ones (gaps), fill whyItMatters (one sentence) and suggestedStep (a specific course, project, certification, or a reframe of existing experience). For strong matches, leave whyItMatters and suggestedStep empty.
            5. severity: "critical" (required & core), "important" (expected), or "nice-to-have" — use mainly for gaps. The Experience level item is "critical" whenever there is a seniority shortfall.
               category: "skill" | "experience" | "education" | "certification".
               actionType: "course" | "project" | "certification" | "reframe".
            6. overallScore (0-100): overall match, weighting critical requirements (including seniority) more.
            7. overallSummary: 1-2 sentences on the match and the top focus areas.

            Rules (follow strictly):
            - List ONLY skills/requirements that THIS JOB needs (from its title, seniority, and description). Never list skills the job does not require.
            - matchPercent reflects how well the CANDIDATE meets each requirement, based ONLY on the profile provided below.
            - NEVER assume the candidate has a skill, tool, or experience that is not explicitly stated in their profile.
            - If the profile provides no evidence for a requirement (e.g. it says "(none provided)"), matchPercent MUST be low (0-15).
            - Do not invent candidate skills, experience, or qualifications.

            JOB:
            Title: {job.Title} (Seniority: {job.Seniority})
            Description: {job.Description}

            CANDIDATE:
            Current title: {Or(profile.JobTitle ?? "", "(none provided)")}
            Total years of professional experience: {totalYears}
            Summary: {Or(profile.Summary ?? "", "(none provided)")}
            Skills: {Or(skills, "(none provided)")}
            Experience: {Or(experience, "(none provided)")}
            Education: {Or(education, "(none provided)")}
            Certifications: {Or(certs, "(none provided)")}
            Projects: {Or(projects, "(none provided)")}
            """;

        var ai = (await _chat.GetResponseAsync<SkillAnalysisAi>(prompt, useJsonSchemaResponseFormat: false)).Result;


        // upsert
        if (analysis is null)
        {
            analysis = new SkillGapAnalysis { UserId = userId, JobId = jobId };
            _db.SkillGapAnalyses.Add(analysis);
        }
        else
        {
            _db.SkillGapItems.RemoveRange(analysis.SkillGapItems);
            analysis.SkillGapItems.Clear();
        }

        analysis.OverallScore = ai.OverallScore;
        analysis.OverallSummary = ai.OverallSummary;
        analysis.ProfileHash = currentHash;
        analysis.UpdatedAt = DateTime.UtcNow;
        foreach (var i in ai.Items)
            analysis.SkillGapItems.Add(new SkillGapItem
            {
                SkillName = i.SkillName,
                MatchPercent = i.MatchPercent,
                Severity = i.Severity,
                Category = i.Category,
                ActionType = i.ActionType,
                WhyItMatters = i.WhyItMatters,
                SuggestedStep = i.SuggestedStep
            });

        await _db.SaveChangesAsync();
        return Result<SkillAnalysisDto>.Success(MapToDto(job, analysis), "Skill analysis complete.", 200);
    }

    // critical requirements 
    private static int SeverityWeight(string severity) => severity?.ToLowerInvariant() switch
    {
        "critical" => 3,
        "important" => 2,
        _ => 1
    };

    private static SkillAnalysisDto MapToDto(Job job, SkillGapAnalysis a)
    {
        var totalWeight = a.SkillGapItems.Sum(i => SeverityWeight(i.Severity));
        var averageMatch = totalWeight == 0
            ? 0
            : (int)Math.Round(a.SkillGapItems.Sum(i => SeverityWeight(i.Severity) * (double)i.MatchPercent) / totalWeight);

        return new SkillAnalysisDto
        {
            JobId = job.Id,
            JobTitle = job.Title,
            OverallScore = a.OverallScore,
            AverageMatch = averageMatch,   
            OverallSummary = a.OverallSummary,
            UpdatedAt = a.UpdatedAt,
            Items = a.SkillGapItems
                .OrderByDescending(i => i.MatchPercent)
                .Select(i => new SkillAssessmentDto
                {
                    SkillName = i.SkillName,
                    MatchPercent = i.MatchPercent,
                    Severity = i.Severity,
                    Category = i.Category,
                    ActionType = i.ActionType,
                    WhyItMatters = i.WhyItMatters,
                    SuggestedStep = i.SuggestedStep
                }).ToList()
        };
    }
}