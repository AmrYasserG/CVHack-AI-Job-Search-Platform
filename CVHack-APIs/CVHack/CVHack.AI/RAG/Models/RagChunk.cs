namespace CVHack.AI
{
    public record RagChunk
    {
        public Guid Id { get; init; }

        public string KnowledgeBase { get; init; } = string.Empty;

        public string Category { get; init; } = string.Empty;

        public string SourceFile { get; init; } = string.Empty;

        public string Text { get; init; } = string.Empty;

        public Dictionary<string, string> Metadata { get; init; } = [];

        public float[] Embedding { get; init; } = [];
    }
}
