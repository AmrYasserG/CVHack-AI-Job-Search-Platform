using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CVHack.AI;

public class RagIngestionHostedService : BackgroundService
{
    private readonly IRagIngestionService _ingestion;
    private readonly ILogger<RagIngestionHostedService> _logger;

    public RagIngestionHostedService(
        IRagIngestionService ingestion,
        ILogger<RagIngestionHostedService> logger)
    {
        _ingestion = ingestion;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _ingestion.IngestAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RAG ingestion failed in background.");
        }
    }
}