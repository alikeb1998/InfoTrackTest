using SECrawler.Domain;
using SECrawler.Domain.Entities;

namespace SECrawler.Application.Services;

public interface ISearchResultRepository
{
    Task<List<SearchResult>> GetHistoriesAsync();

    Task<bool> AddResultAsync(List<int> ranks, string keywords, EngineType engineType, CancellationToken cancellationToken);
}