using CVHack.AI;
using CVHack.Common;
using CVHack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace CVHack.BLL;

public class CompanyResearchService : ICompanyResearchService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _db;
    private readonly IWebSearchClient _search;
    private readonly IChatClient _chat;

    public CompanyResearchService(IUnitOfWork uow, AppDbContext db, IWebSearchClient search, IChatClient chat)
    {
        _uow = uow; _db = db; _search = search; _chat = chat;
    }

    public async Task<Result<CompanyBriefingDto>> GetBriefingAsync(int jobId)
    {
        var job = await _uow.JobRepository.GetByIdAsync(jobId);
        if (job is null)
            return Result<CompanyBriefingDto>.Failure("Job not found.", "No such job.", 404);

        var company = job.CompanyName;

        // 1. cache (fresh for 7 days)
        var cached = await _db.CompanyBriefings.FirstOrDefaultAsync(c => c.CompanyName == company);
        if (cached is not null && cached.UpdatedAt > DateTime.UtcNow.AddDays(-7))
            return Result<CompanyBriefingDto>.Success(MapToDto(cached));

        // 2. web search
        var context = await _search.SearchAsync(
    $"          {company} company profile: year founded, headquarters, number of employees, what they do, culture");

        // 3. LLM extracts the structured fields
        var prompt = $"""
        Using ONLY the search results below, extract structured info about the company "{company}".
        - founded: the year it was founded (a number like 2011), or null if not found.
        - staffMin and staffMax: the approximate employee count range as numbers
          (e.g. 100 and 500). If only one number is found, use it for both. null if unknown.
        - content: 4-5 short bullet points covering what the company does, its culture, and work
        environment. Return content as an array of strings, each one a single concise bullet (no leading dashes).

        SEARCH RESULTS:
        {context}
        """;

        var ai = (await _chat.GetResponseAsync<CompanyBriefingAi>(prompt, useJsonSchemaResponseFormat: false)).Result;

        // 4. upsert into the cache
        if (cached is null)
        {
            cached = new CompanyBriefing { CompanyName = company };
            _db.CompanyBriefings.Add(cached);
        }
        cached.Founded = ai.Founded;
        cached.StaffMin = ai.StaffMin;
        cached.StaffMax = ai.StaffMax;
        cached.Content = JsonSerializer.Serialize(ai.Content);
        cached.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Result<CompanyBriefingDto>.Success(MapToDto(cached), "Briefing generated.", 200);
    }

    private static CompanyBriefingDto MapToDto(CompanyBriefing c) => new()
    {
        Id = c.Id,
        CompanyName = c.CompanyName,
        Founded = c.Founded,
        StaffMin = c.StaffMin,
        StaffMax = c.StaffMax,
        Content = string.IsNullOrEmpty(c.Content)
    ? new List<string>()
    : JsonSerializer.Deserialize<List<string>>(c.Content)!,
        UpdatedAt = c.UpdatedAt
    };
}