using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CVHack.AI;

public class JoobleClient : IJoobleClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private static readonly JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web);

    public JoobleClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Jooble:ApiKey"]
            ?? throw new InvalidOperationException("Jooble:ApiKey is missing. Add it to appsettings.json or user-secrets.");
    }

    public async Task<List<JoobleJob>> FetchJobsAsync(string keywords, int maxResults = 15, CancellationToken ct = default)
    {
        var url = $"https://jooble.org/api/{_apiKey}";
        var body = new { keywords, page = 1, resultsOnPage = maxResults };

        var response = await _http.PostAsJsonAsync(url, body, ct);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<JoobleResponse>(_opts, ct);
        return data?.Jobs ?? [];
    }

    private record JoobleResponse(List<JoobleJob> Jobs);
}
