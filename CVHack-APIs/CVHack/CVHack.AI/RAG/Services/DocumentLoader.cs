using Microsoft.Extensions.Configuration;

namespace CVHack.AI;

public class DocumentLoader : IDocumentLoader
{
    private readonly string _knowledgeBasePath;

    public DocumentLoader(IConfiguration configuration)
    {
        var relativePath = configuration["Rag:KnowledgeBasePath"]
            ?? throw new InvalidOperationException("Rag:KnowledgeBasePath is not configured.");

        _knowledgeBasePath = Path.Combine(AppContext.BaseDirectory, relativePath);
    }

    public async Task<IReadOnlyList<Document>> LoadAsync()
    {
        var documents = new List<Document>();

        if (!Directory.Exists(_knowledgeBasePath))
            return documents;

        foreach (var kbDirectory in Directory.GetDirectories(_knowledgeBasePath))
        {
            var knowledgeBase = Path.GetFileName(kbDirectory);

            foreach (var categoryDirectory in Directory.GetDirectories(kbDirectory))
            {
                var category = Path.GetFileName(categoryDirectory);

                foreach (var file in Directory.GetFiles(categoryDirectory, "*.txt"))
                {
                    var content = await File.ReadAllTextAsync(file);

                    if (string.IsNullOrWhiteSpace(content))
                        continue;

                    documents.Add(new Document
                    {
                        KnowledgeBase = knowledgeBase,
                        Category = category,
                        FileName = Path.GetFileName(file),
                        Content = content
                    });
                }
            }
        }

        return documents;
    }
}