using Microsoft.EntityFrameworkCore;
using SECrawler.Application.Services;
using SECrawler.Domain;
using SECrawler.Domain.Entities;
using SECrawler.Infrastructure.Data;

namespace SECrawler.Infrastructure.Repository;

public class SearchResultRepository(EfDbContext dbContext) : ISearchResultRepository
{
    public async Task<List<SearchResult>> GetHistoriesAsync()
    {
        return await dbContext.SearchResults.ToListAsync();
    }

    public async Task<bool> AddResultAsync(List<int> rankings, string keywords, EngineType engineType, CancellationToken cancellationToken)
    {
        var result = new SearchResult()
        {
            EngineType = engineType.ToString(),
            KeyWords = keywords,
            Date = DateTime.Now,
            Rank = string.Join(",", rankings.Select(x => x.ToString())),
        };

        await dbContext.SearchResults.AddAsync(result, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}