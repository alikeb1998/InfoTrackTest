namespace SECrawler.Application.Services.Implementations;

public class HttpClientWrapper(HttpClient httpClient) : IHttpClientWrapper
{
    public async Task<string> GetStringAsync(string url, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var (key, value) in headers)
            request.Headers.Add(key, value);

        var response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}