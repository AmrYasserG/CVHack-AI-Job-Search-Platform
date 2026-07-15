namespace CVHack.AI;

public interface IRagIngestionService
{
    Task IngestAsync(CancellationToken ct = default);
}