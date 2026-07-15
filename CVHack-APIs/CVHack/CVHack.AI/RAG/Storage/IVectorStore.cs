namespace CVHack.AI;

public interface IVectorStore
{
    Task UpsertAsync(RagChunk chunk, CancellationToken ct = default);
    Task<IReadOnlyList<SearchResult>> SearchAsync(float[] queryVector, string knowledgeBase, int topK = 5, CancellationToken ct = default);
}