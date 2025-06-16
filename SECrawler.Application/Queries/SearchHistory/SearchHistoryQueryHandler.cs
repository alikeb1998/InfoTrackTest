using FluentValidation;
using MediatR;
using SECrawler.Application.Services;
using SECrawler.Domain;

namespace SECrawler.Application.Queries.SearchHistory;

public class SearchHistoryQueryHandler(ISearchResultRepository repository):IRequestHandler<SearchHistoryQuery, ApiResult<List<SearchResultDto>>>
{
    public async Task<ApiResult<List<SearchResultDto>>> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetHistoriesAsync();
        //TODO:Use auto mapper instead
        var result = items.Select(x => new SearchResultDto()
        {
            Id = x.Id,
            Date = x.Date,
            Rank = x.Rank,
            EngineType = x.EngineType,
            KeyWords = x.KeyWords,
        }).ToList();
        return ApiResult<List<SearchResultDto>>.Success(result);
    }
}