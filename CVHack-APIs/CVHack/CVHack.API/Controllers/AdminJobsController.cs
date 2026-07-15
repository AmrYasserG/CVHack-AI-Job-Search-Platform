using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API.Controllers;

[ApiController]
[Route("api/admin/jobs")]
[Authorize(Policy = "AdminOnly")]
public class AdminJobsController : ControllerBase
{
    private readonly JobIngestionHostedService _ingestion;
    private readonly Microsoft.Extensions.Hosting.IHostApplicationLifetime _lifetime;

    public AdminJobsController(JobIngestionHostedService ingestion, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime)
    {
        _ingestion = ingestion;
        _lifetime = lifetime;
    }

    [HttpPost("ingest")]
    public IActionResult TriggerIngestion([FromQuery] string? roleTitle)
    {
        if (JobIngestionHostedService.IsRunning)
            return Conflict(new { message = "Ingestion is already running." });

        _ = _ingestion.RunOneSafeAsync(roleTitle, _lifetime.ApplicationStopping);
        return Accepted(new { message = "Ingestion started in background." });
    }
}
