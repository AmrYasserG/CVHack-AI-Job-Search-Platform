namespace CVHack.BLL;

public record IngestionRunSummary(int Fetched, int Inserted, int Reactivated, int Expired, int Skipped);

public interface IJobIngestionService
{
    Task<IngestionRunSummary> RunAsync(string? roleTitle = null, CancellationToken ct = default);
}
