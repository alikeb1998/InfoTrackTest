using MediatR;
using SECrawler.Domain;

namespace SECrawler.Application.Queries.SearchHistory;

public class SearchHistoryQuery: IRequest<ApiResult<List<SearchResultDto>>>
{
    
}