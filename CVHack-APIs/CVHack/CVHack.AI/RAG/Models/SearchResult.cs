namespace CVHack.AI
{
    public class SearchResult
    {
        public RagChunk Chunk { get; init; } = default!;

        public double Score { get; init; }
    }
}
