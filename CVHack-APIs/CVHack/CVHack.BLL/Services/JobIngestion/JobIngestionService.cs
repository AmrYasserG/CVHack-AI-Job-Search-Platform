using CVHack.AI;
using CVHack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CVHack.BLL;

public class JobIngestionService : IJobIngestionService
{
    private readonly AppDbContext _db;
    private readonly IJoobleClient _jooble;
    private readonly IJobNormalizationService _normalizer;
    private readonly ILogger<JobIngestionService> _logger;
    private readonly int _maxPerTitle;
    private readonly int _ttlDays;

    public JobIngestionService(AppDbContext db, IJoobleClient jooble,
        IJobNormalizationService normalizer, ILogger<JobIngestionService> logger,
        IConfiguration config)
    {
        _db = db; _jooble = jooble; _normalizer = normalizer; _logger = logger;
        _maxPerTitle = config.GetValue<int>("Jooble:MaxJobsPerTitle", 15);
        _ttlDays     = config.GetValue<int>("Jooble:JobTtlDays", 30);
    }

    public async Task<IngestionRunSummary> RunAsync(string? roleTitle = null, CancellationToken ct = default)
    {
        int fetched = 0, inserted = 0, reactivated = 0, expired = 0, skipped = 0;

        var dbQuery = _db.SupportedRoles.Where(r => r.IsActive);
        if (!string.IsNullOrWhiteSpace(roleTitle))
        {
            dbQuery = dbQuery.Where(r => r.Title == roleTitle);
        }

        var roles = await dbQuery.ToListAsync(ct);
        _logger.LogInformation("Ingestion starting — {N} active roles.", roles.Count);

        foreach (var role in roles)
        {
            if (ct.IsCancellationRequested) break;
            var query = role.SearchQuery ?? role.Title;

            try
            {
                var rawJobs = await _jooble.FetchJobsAsync(query, _maxPerTitle, ct);
                fetched += rawJobs.Count;

                // 1. Filter out duplicate jobs returned in the same API response itself
                var uniqueRawJobs = new List<JoobleJob>();
                var seenIds = new HashSet<string>();
                var seenUrls = new HashSet<string>();
                foreach (var raw in rawJobs)
                {
                    var extId = raw.Id?.ToString() ?? raw.Link;
                    if (seenIds.Add(extId) && seenUrls.Add(raw.Link))
                    {
                        uniqueRawJobs.Add(raw);
                    }
                }

                var candidateIds = uniqueRawJobs.Select(raw => raw.Id?.ToString() ?? raw.Link).ToList();
                var candidateUrls = uniqueRawJobs.Select(raw => raw.Link).ToList();

                // 2. Fetch existing matching jobs in a single DB query (N+1 query optimization)
                var existingJobs = await _db.Jobs
                    .Where(j => j.SourcePlatform == "Jooble" &&
                               ((j.ExternalId != null && candidateIds.Contains(j.ExternalId)) || candidateUrls.Contains(j.JobUrl)))
                    .ToListAsync(ct);

                int batchInserted = 0, batchReactivated = 0, batchSkipped = 0;

                int totalJobs = uniqueRawJobs.Count;
                int currentIdx = 0;

                foreach (var raw in uniqueRawJobs)
                {
                    currentIdx++;
                    var externalId = raw.Id?.ToString() ?? raw.Link;

                    // check duplicates locally in-memory
                    var existing = existingJobs.FirstOrDefault(j => j.ExternalId == externalId || j.JobUrl == raw.Link);

                    if (existing is not null)
                    {
                        existing.LastSeenAt = DateTime.UtcNow;
                        if (!existing.IsActive) { existing.IsActive = true; batchReactivated++; }
                        else batchSkipped++;
                        continue;
                    }

                    _logger.LogInformation("Normalizing job {Current}/{Total}: '{Title}'...", currentIdx, totalJobs, raw.Title);
                    var normalized = await _normalizer.NormalizeAsync(raw, ct);
                    if (normalized is null) 
                    {
                        _logger.LogWarning("Skipped job '{Title}' because normalization failed.", raw.Title);
                        batchSkipped++; 
                        continue; 
                    }

                    var job = new Job
                    {
                        ExternalId       = externalId,
                        Title            = raw.Title,
                        CompanyName      = !string.IsNullOrWhiteSpace(raw.Company) ? raw.Company : "Unknown",
                        SourcePlatform   = "Jooble",
                        Description      = !string.IsNullOrWhiteSpace(normalized.BriefDescription)
                                           ? normalized.BriefDescription
                                           : raw.Title,
                        BriefDescription = normalized.BriefDescription,
                        City             = normalized.City,
                        Country          = normalized.Country,
                        Seniority        = normalized.Seniority,
                        WorkType         = normalized.WorkType,
                        WorkTime         = normalized.WorkTime,
                        JobUrl           = raw.Link,
                        SalaryMin        = normalized.SalaryMin,
                        SalaryMax        = normalized.SalaryMax,
                        PostedAt         = DateTime.TryParse(raw.Updated, out var d) ? d : DateTime.UtcNow,
                        IsActive         = true,
                        LastSeenAt       = DateTime.UtcNow,
                        Responsibilities = System.Text.Json.JsonSerializer.Serialize(normalized.Responsibilities),
                        Requirements     = System.Text.Json.JsonSerializer.Serialize(normalized.Requirements)
                    };

                    _db.Jobs.Add(job);
                    batchInserted++;
                }

                // 3. Batch save changes for the role (N+1 SaveChanges optimization)
                role.LastIngestedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                // Batch succeeded, accumulate metrics
                inserted += batchInserted;
                reactivated += batchReactivated;
                skipped += batchSkipped;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error saving batch for role '{R}'. Clearing change tracker.", role.Title);
                _db.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ingesting role '{R}'.", role.Title);
            }
        }

        // Expire jobs not seen in TTL days
        var cutoff = DateTime.UtcNow.AddDays(-_ttlDays);
        expired = await _db.Jobs
            .Where(j => j.IsActive && j.LastSeenAt < cutoff)
            .ExecuteUpdateAsync(s => s.SetProperty(j => j.IsActive, false), ct);

        _logger.LogInformation(
            "Done. Fetched={F} Inserted={I} Reactivated={R} Expired={E} Skipped={S}",
            fetched, inserted, reactivated, expired, skipped);

        return new IngestionRunSummary(fetched, inserted, reactivated, expired, skipped);
    }

    
}
