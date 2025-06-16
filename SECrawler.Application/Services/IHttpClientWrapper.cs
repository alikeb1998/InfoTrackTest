namespace SECrawler.Application.Services;

public interface IHttpClientWrapper
{
    Task<string> GetStringAsync(string url, Dictionary<string, string> headers);  
}