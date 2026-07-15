using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVHack.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddAiIntegrations(this IServiceCollection services, IConfiguration config)
    {
        services.AddGroqChatClient(config);     // LLM (reasoning agents)

        services.AddHttpClient<IWebSearchClient, TavilySearchClient>();   // web search

        // RAG
        services.AddHttpClient<IEmbeddingService, GeminiEmbeddingService>();
        services.AddSingleton<IDocumentLoader, DocumentLoader>();
        services.AddSingleton<TextChunker>();
        services.AddSingleton<IVectorStore, InMemoryVectorStore>();
        services.AddSingleton<IRagIngestionService, RagIngestionService>();
        services.AddHostedService<RagIngestionHostedService>();
        services.AddSingleton<IRagService, RagService>();

        services.AddHttpClient<IJoobleClient, JoobleClient>();
        services.AddScoped<IJobNormalizationService, JobNormalizationService>();

        return services;
    }
}
