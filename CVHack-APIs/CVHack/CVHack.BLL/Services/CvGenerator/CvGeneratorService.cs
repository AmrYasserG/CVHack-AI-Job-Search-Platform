using CVHack.AI;
using CVHack.BLL.DTOs;
using CVHack.DAL;
using Microsoft.Extensions.AI;

namespace CVHack.BLL.Services.CvGenerator;

public class CvGeneratorService : ICvGeneratorService
{
    private readonly IChatClient _chat;
    private readonly IProfileRepository _profileRepo;
    private readonly IRagService _rag;
    private readonly IJobManager _jobManager;
    private readonly ISkillAnalysisService _skillAnalysis;

    public CvGeneratorService(IChatClient chat, IProfileRepository profileRepo, IRagService rag, IJobManager jobManager, ISkillAnalysisService skillAnalysis)
    {
        _chat = chat;
        _profileRepo = profileRepo;
        _rag = rag;
        _jobManager = jobManager;
        _skillAnalysis = skillAnalysis;
    }

    public async Task<GenerateCvResponseDto> GenerateAsync(string userId, int jobId)
    {
        var profile = await _profileRepo.GetFullProfileAsync(userId);
        if (profile == null) throw new Exception("Profile not found");

        var jobResult = await _jobManager.GetJobByIdAsync(jobId);
        if (!jobResult.IsSuccess) throw new Exception("Job not found");
        var job = jobResult.Data!;

        var analysisResult = await _skillAnalysis.AnalyzeAsync(jobId, userId);
        if (analysisResult.IsSuccess && analysisResult.Data?.OverallScore < 50)
            throw new Exception($"Your match score is {analysisResult.Data?.OverallScore}%. A minimum of 50% is required to generate a CV for this job.");

        var profileText = BuildProfileText(profile);

        string cvExamples = string.Empty;
        try
        {
            cvExamples = await _rag.SearchAsync(
                query: $"{job.Title} CV example",
                knowledgeBase: "CVs",
                topK: 3
            );
        }
        catch
        {
            throw new Exception("CV examples are currently unavailable. Please try again later.");
        }

        if (!cvExamples.Contains("[1]"))
            throw new Exception("No CV examples found for this job type. Please try again later.");

        var prompt = $"""
            You are a professional CV writer.

            Here are some strong CV examples for reference style only:
            {cvExamples}

            Create a tailored, well-formatted CV for this candidate.

            TARGET JOB:
            Title: {job.Title}
            Description: {job.Description}

            CANDIDATE PROFILE:
            {profileText}

            STRICT RULES:
            - Use ONLY the information provided in the candidate profile above
            - Do NOT invent, assume, or add any skills, experiences, or qualities not mentioned
            - If a section has no data, omit the section entirely
            - Do NOT add suggestions or notes
            - Tailor the existing skills and experience to highlight relevance to the job

            FORMAT RULES:
            - Start with candidate name and contact info (email, phone, location, LinkedIn, GitHub, portfolio)
            - Sections: SUMMARY | EXPERIENCE | EDUCATION | SKILLS | PROJECTS | CERTIFICATIONS
            - Use clean plain text with clear section headers in ALL CAPS
            - Under Experience: include job title, company, dates, and bullet points for responsibilities
            - Under Projects: include project name, tech stack if available, and key contributions
            - Keep it concise and ATS-friendly
            - No markdown symbols like # or *
            """;

        var response = await _chat.GetResponseAsync(prompt);
        return new GenerateCvResponseDto { CvText = response.Text ?? string.Empty };
    }

    private string BuildProfileText(UserProfile profile)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"Name: {profile.User.FirstName} {profile.User.LastName}");
        sb.AppendLine($"Email: {profile.User.Email}");

        if (!string.IsNullOrEmpty(profile.PhoneNumber))
            sb.AppendLine($"Phone: {profile.PhoneNumber}");

        if (!string.IsNullOrEmpty(profile.City) || !string.IsNullOrEmpty(profile.Country))
            sb.AppendLine($"Location: {profile.City}, {profile.Country}");

        if (!string.IsNullOrEmpty(profile.LinkedInUrl))
            sb.AppendLine($"LinkedIn: {profile.LinkedInUrl}");

        if (!string.IsNullOrEmpty(profile.GitHubUrl))
            sb.AppendLine($"GitHub: {profile.GitHubUrl}");

        if (!string.IsNullOrEmpty(profile.PortfolioUrl))
            sb.AppendLine($"Portfolio: {profile.PortfolioUrl}");

        if (!string.IsNullOrEmpty(profile.Headline))
            sb.AppendLine($"Headline: {profile.Headline}");

        if (!string.IsNullOrEmpty(profile.Summary))
            sb.AppendLine($"Summary: {profile.Summary}");

        if (profile.Experiences.Any())
        {
            sb.AppendLine("\nEXPERIENCE:");
            foreach (var exp in profile.Experiences)
            {
                var end = exp.EndDate.HasValue
                    ? exp.EndDate.Value.ToString("MMM yyyy")
                    : "Present";
                sb.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {end})");
            }
        }

        if (profile.Educations.Any())
        {
            sb.AppendLine("\nEDUCATION:");
            foreach (var edu in profile.Educations)
                sb.AppendLine($"- {edu.Degree} at {edu.University} ({edu.StartYear} - {edu.EndYear})");
        }

        if (profile.ProfileSkills.Any())
        {
            sb.AppendLine("\nSKILLS:");
            sb.AppendLine(string.Join(", ", profile.ProfileSkills.Select(s => s.Skill.Name)));
        }

        if (profile.Projects.Any())
        {
            sb.AppendLine("\nPROJECTS:");
            foreach (var proj in profile.Projects)
                sb.AppendLine($"- {proj.Title}: {proj.Description}");
        }

        if (profile.Certifications.Any())
        {
            sb.AppendLine("\nCERTIFICATIONS:");
            foreach (var cert in profile.Certifications)
                sb.AppendLine($"- {cert.Name} ({cert.Provider})");
        }

        return sb.ToString();
    }
}