using MediatR;
using SECrawler.Domain;

namespace SECrawler.Application.Commands.Search;

public class SearchCommand: IRequest<ApiResult<List<int>>>
{
    public string Query { get; set; } = null!;
    public int PageSize { get; set; }
    public int EngineId { get; set; }
}