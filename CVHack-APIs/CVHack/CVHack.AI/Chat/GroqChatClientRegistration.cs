using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using System.ClientModel;

namespace CVHack.AI;

// Registers the LLM chat client (Groq, via its OpenAI-compatible endpoint).
// Used by the agents that reason in words: Company Researcher, Skill Analysis, etc.
public static class GroqChatClientRegistration
{
    private const string Endpoint = "https://api.groq.com/openai/v1";
    private const string Model = "llama-3.3-70b-versatile";

    public static IServiceCollection AddGroqChatClient(this IServiceCollection services, IConfiguration config)
    {
        var apiKey = config["Groq:ApiKey"]
            ?? throw new InvalidOperationException("Groq:ApiKey is missing (set it with dotnet user-secrets).");

        var client = new OpenAIClient(
            new ApiKeyCredential(apiKey),
            new OpenAIClientOptions { Endpoint = new Uri(Endpoint) });

        services.AddChatClient(client.GetChatClient(Model).AsIChatClient());

        return services;
    }
}
