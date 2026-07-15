using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CVHack.AI;

public class GeminiEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private const string Model = "gemini-embedding-001";

    public GeminiEmbeddingService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini:ApiKey is not configured.");
    }

    public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:embedContent?key={_apiKey}";

        var body = new
        {
            model = $"models/{Model}",
            content = new { parts = new[] { new { text } } }
        };

        var retries = 5;
        var delay = 5000;

        for (var attempt = 0; attempt < retries; attempt++)
        {
            var response = await _http.PostAsJsonAsync(url, body, ct);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
                return json
                    .GetProperty("embedding")
                    .GetProperty("values")
                    .EnumerateArray()
                    .Select(v => v.GetSingle())
                    .ToArray();
            }

            if (attempt < retries - 1 &&
                ((int)response.StatusCode == 429 || (int)response.StatusCode == 503))
            {
                await Task.Delay(delay, ct);
                delay *= 2;
                continue;
            }

            response.EnsureSuccessStatusCode();
        }

        throw new HttpRequestException("Embedding failed after all retries.");
    }
}