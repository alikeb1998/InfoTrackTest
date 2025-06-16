using MediatR;
using SECrawler.Domain;
using SECrawler.Domain.Entities;

namespace SECrawler.Application.Queries.Engines;

public class EngineListQuery : IRequest<ApiResult<List<Engine>>>
{
    
}