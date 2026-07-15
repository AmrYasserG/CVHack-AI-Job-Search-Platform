using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CVHack.AI;

public class RagIngestionService : IRagIngestionService
{
    private readonly IDocumentLoader _loader;
    private readonly TextChunker _chunker;
    private readonly IEmbeddingService _embedder;
    private readonly IVectorStore _store;
    private readonly ILogger<RagIngestionService> _logger;

    private readonly string _cachePath = Path.Combine(AppContext.BaseDirectory, "rag_cache.json");

    public RagIngestionService(
        IDocumentLoader loader,
        TextChunker chunker,
        IEmbeddingService embedder,
        IVectorStore store,
        ILogger<RagIngestionService> logger)
    {
        _loader = loader;
        _chunker = chunker;
        _embedder = embedder;
        _store = store;
        _logger = logger;
    }

    public async Task IngestAsync(CancellationToken ct = default)
    {
        // try loading from cache first
        if (File.Exists(_cachePath))
        {
            _logger.LogInformation("RAG cache found. Loading from disk...");
            await LoadFromCacheAsync(ct);
            return;
        }

        // no cache — run full ingestion
        _logger.LogInformation("No RAG cache found. Running full ingestion...");
        var chunks = await RunIngestionAsync(ct);

        // save to cache for next startup
        await SaveToCacheAsync(chunks, ct);

        _logger.LogInformation("RAG ingestion complete. Total chunks: {Total}", chunks.Count);
    }

    private async Task<List<RagChunk>> RunIngestionAsync(CancellationToken ct)
    {
        var documents = await _loader.LoadAsync();
        var allChunks = new List<RagChunk>();

        foreach (var document in documents)
        {
            var chunks = _chunker.Chunk(document).ToList();

            foreach (var chunk in chunks)
            {
                var embedding = await _embedder.EmbedAsync(chunk.Text, ct);
                var embeddedChunk = chunk with { Embedding = embedding };

                await _store.UpsertAsync(embeddedChunk, ct);
                allChunks.Add(embeddedChunk);

                await Task.Delay(2500, ct);
            }

            _logger.LogInformation(
                "Ingested {Count} chunks from {KB}/{Category}/{File}",
                chunks.Count,
                document.KnowledgeBase,
                document.Category,
                document.FileName);
        }

        return allChunks;
    }

    private async Task SaveToCacheAsync(List<RagChunk> chunks, CancellationToken ct)
    {
        _logger.LogInformation("Saving RAG cache to disk...");
        var json = JsonSerializer.Serialize(chunks);
        await File.WriteAllTextAsync(_cachePath, json, ct);
        _logger.LogInformation("RAG cache saved: {Path}", _cachePath);
    }

    private async Task LoadFromCacheAsync(CancellationToken ct)
    {
        var json = await File.ReadAllTextAsync(_cachePath, ct);
        var chunks = JsonSerializer.Deserialize<List<RagChunk>>(json)!;

        foreach (var chunk in chunks)
            await _store.UpsertAsync(chunk, ct);

        _logger.LogInformation("Loaded {Total} chunks from cache.", chunks.Count);
    }
}