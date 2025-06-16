using MediatR;
using SECrawler.Domain;

namespace SECrawler.Application.Commands.Results;

public class SaveResultsCommand : IRequest<ApiResult<bool>>
{
    public string KeyWord { get; set; } = null!;
    public EngineType Engine { get; set; }
    public List<int> Rankings { get; set; } = new List<int>();
}