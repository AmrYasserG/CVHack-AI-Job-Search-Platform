namespace CVHack.AI;

public class InMemoryVectorStore : IVectorStore
{
    private readonly List<RagChunk> _chunks = new();

    public Task UpsertAsync(RagChunk chunk, CancellationToken ct = default)
    {
        _chunks.Add(chunk);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<SearchResult>> SearchAsync(
        float[] queryVector,
        string knowledgeBase,
        int topK = 5,
        CancellationToken ct = default)
    {
        var results = _chunks
            .Where(c => c.KnowledgeBase == knowledgeBase)
            .Select(c => new SearchResult
            {
                Chunk = c,
                Score = CosineSimilarity(queryVector, c.Embedding)
            })
            .OrderByDescending(r => r.Score)
            .Take(topK)
            .ToList();

        return Task.FromResult<IReadOnlyList<SearchResult>>(results);
    }

    private static double CosineSimilarity(float[] a, float[] b)
    {
        var dot = 0.0;
        var magA = 0.0;
        var magB = 0.0;

        for (var i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return magA == 0 || magB == 0 ? 0 : dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}