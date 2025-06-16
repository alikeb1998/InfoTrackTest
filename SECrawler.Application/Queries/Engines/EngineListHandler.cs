using MediatR;
using SECrawler.Application.Services;
using SECrawler.Domain;
using SECrawler.Domain.Entities;

namespace SECrawler.Application.Queries.Engines;

public class EngineListHandler (IEngineRepository repository) : IRequestHandler<EngineListQuery, ApiResult<List<Engine>>>
{
    public async Task<ApiResult<List<Engine>>> Handle(EngineListQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetEnginesAsync();
        return ApiResult<List<Engine>>.Success(items.ToList());
    }
}