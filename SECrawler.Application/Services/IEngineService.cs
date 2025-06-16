namespace SECrawler.Application.Services;

public interface IEngineService
{
    Task<List<int>> GetRankingsAsync(string query, int engineId, int pageSize);
}