namespace CVHack.AI;

public interface IWebSearchClient
{
    Task<string> SearchAsync(string query, CancellationToken ct = default);
}