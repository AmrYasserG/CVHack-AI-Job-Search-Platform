namespace CVHack.AI;

public class RagService : IRagService
{
    private readonly IEmbeddingService _embedder;
    private readonly IVectorStore _store;

    public RagService(IEmbeddingService embedder, IVectorStore store)
    {
        _embedder = embedder;
        _store = store;
    }

    public async Task<string> SearchAsync(string query, string knowledgeBase, int topK = 5, CancellationToken ct = default)
    {
        var queryVector = await _embedder.EmbedAsync(query, ct);

        var results = await _store.SearchAsync(queryVector, knowledgeBase, topK, ct);

        return string.Join("\n\n", results.Select((r, i) =>
            $"[{i + 1}] (Score: {r.Score:F3}) [{r.Chunk.Category}/{r.Chunk.SourceFile}]\n{r.Chunk.Text}"));
    }
}