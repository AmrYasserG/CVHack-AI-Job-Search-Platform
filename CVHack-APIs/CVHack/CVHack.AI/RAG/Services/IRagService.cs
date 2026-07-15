namespace CVHack.AI;

public interface IRagService
{
    Task<string> SearchAsync(string query, string knowledgeBase, int topK = 5, CancellationToken ct = default);
}