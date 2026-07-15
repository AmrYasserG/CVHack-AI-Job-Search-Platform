using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CVHack.BLL;

public class JobIngestionHostedService : BackgroundService
{
    public static bool IsRunning = false;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobIngestionHostedService> _logger;
    private readonly TimeSpan _interval;

    public JobIngestionHostedService(IServiceScopeFactory scopeFactory,
        ILogger<JobIngestionHostedService> logger, IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _interval = TimeSpan.FromHours(config.GetValue<double>("Jooble:IngestionIntervalHours", 12));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_interval);
        while (!stoppingToken.IsCancellationRequested)
        {
            //for seach only one role title, you can remove the parameter to run for all active roles
            await RunOneSafeAsync("FrontendReact", stoppingToken); 
            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }

    public async Task RunOneSafeAsync(string? roleTitle = null, CancellationToken ct = default)
    {
        if (IsRunning) { _logger.LogWarning("Already running — skipping."); return; }

        IsRunning = true;
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IJobIngestionService>();
            await svc.RunAsync(roleTitle, ct);
        }
        finally { IsRunning = false; }
    }
}
