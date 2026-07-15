namespace CVHack.AI;

public interface IJoobleClient
{
    Task<List<JoobleJob>> FetchJobsAsync(string keywords, int maxResults = 15, CancellationToken ct = default);
}

public record JoobleJob(
    long? Id,
    string Title,
    string Company,
    string Location,
    string Type,
    string Snippet,
    string Link,
    string Updated);
