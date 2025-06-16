using System.Text.RegularExpressions;
using System.Web;
using SECrawler.Domain.Extensions;

namespace SECrawler.Application.Services.Implementations;

public class BingEngineService(IEngineRepository dataService, IHttpClientWrapper httpClient) : IEngineService
{
    private const string UserAgent = "Mozilla/5.0 (...) Chrome/119.0.0.0 Safari/537.36";

    public async Task<List<int>> GetRankingsAsync(string query, int engineId, int pageSize)
    {
        var engine = await dataService.GetOneAsync(engineId);
        if (engine == null) return [];

        var searchUrl = engine.SearchUrl.Replace("#query#", HttpUtility.UrlEncode(query))
            .Replace("#pageSize#", pageSize.ToString());
        var url = $"{engine.BaseUrl}/{searchUrl}";

        var headers = new Dictionary<string, string> { { "User-Agent", UserAgent } };
        var html = await httpClient.GetStringAsync(url, headers);
        var response = HttpUtility.HtmlDecode(html);

        var links = Extensions.RetrieveLinksFromResponse(response, engine.Expression);
        var ranks = links
            .Select((link, index) => (link, index))
            .Where(t => t.link.Contains("www.infotrack.co.uk", StringComparison.OrdinalIgnoreCase))
            .Select(t => t.index)
            .Distinct()
            .ToList();

        return ranks;
    }
}