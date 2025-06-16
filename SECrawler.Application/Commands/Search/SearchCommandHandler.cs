using FluentValidation;
using MediatR;
using SECrawler.Application.Services;
using SECrawler.Domain;

namespace SECrawler.Application.Commands.Search;

public class SearchCommandHandler(IValidator<SearchCommand> validator, IEngineFactory engineFactory) : IRequestHandler<SearchCommand, ApiResult<List<int>>>
{
    public async Task<ApiResult<List<int>>> Handle(SearchCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            return ApiResult<List<int>>.Fail(validatorResult.Errors?.FirstOrDefault()?.ErrorMessage);
        }
        try
        {
            var service = engineFactory.CreateEngineService(request.EngineId);
            var res = await service.GetRankingsAsync(request.Query, request.EngineId, request.PageSize);
            return ApiResult<List<int>>.Success(res);
        }
        catch (Exception e)
        {
            return ApiResult<List<int>>.Fail(e.Message);
        }
    }
}