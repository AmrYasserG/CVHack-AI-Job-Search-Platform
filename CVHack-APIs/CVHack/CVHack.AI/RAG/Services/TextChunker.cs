namespace CVHack.AI;

public class TextChunker
{
    private readonly int _chunkSize;
    private readonly int _overlap;

    public TextChunker(int chunkSize = 1000, int overlap = 150)
    {
        _chunkSize = chunkSize;
        _overlap = overlap;
    }

    public IEnumerable<RagChunk> Chunk(Document document)
    {
        var text = document.Content.Trim();
        var chunks = new List<RagChunk>();
        var start = 0;
        var index = 0;

        while (start < text.Length)
        {
            var length = Math.Min(_chunkSize, text.Length - start);
            var chunkText = text.Substring(start, length);

            chunks.Add(new RagChunk
            {
                Id = Guid.NewGuid(),
                KnowledgeBase = document.KnowledgeBase,
                Category = document.Category,
                SourceFile = document.FileName,
                Text = chunkText,
                Metadata = new Dictionary<string, string>
                {
                    ["KnowledgeBase"] = document.KnowledgeBase,
                    ["Category"] = document.Category,
                    ["FileName"] = document.FileName,
                    ["ChunkIndex"] = index.ToString()
                }
            });

            start += _chunkSize - _overlap;
            index++;
        }

        return chunks;
    }
}