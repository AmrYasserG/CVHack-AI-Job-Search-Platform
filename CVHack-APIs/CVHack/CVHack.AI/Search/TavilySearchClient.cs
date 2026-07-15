using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace CVHack.AI;

public class TavilySearchClient : IWebSearchClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public TavilySearchClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Tavily:ApiKey"]
            ?? throw new InvalidOperationException("Tavily:ApiKey is missing. Add it to appsettings.json or user-secrets.");
    }

    public async Task<string> SearchAsync(string query, CancellationToken ct = default)
    {
        var body = new
        {
            api_key = _apiKey,
            query,
            max_results = 7,
            search_depth = "advanced",
        };
        var resp = await _http.PostAsJsonAsync("https://api.tavily.com/search", body, ct);
        resp.EnsureSuccessStatusCode();
        var data = await resp.Content.ReadFromJsonAsync<TavilyResponse>(cancellationToken: ct);
        return string.Join("\n\n", data!.Results.Select(r => $"{r.Title}\n{r.Content}"));
    }

    private record TavilyResponse(List<TavilyResult> Results);
    private record TavilyResult(string Title, string Content, string Url);
}