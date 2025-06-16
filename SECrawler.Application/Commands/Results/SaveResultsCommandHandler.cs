using FluentValidation;
using MediatR;
using SECrawler.Application.Services;
using SECrawler.Domain;

namespace SECrawler.Application.Commands.Results;

public class SaveResultsCommandHandler(IValidator<SaveResultsCommand> validator, ISearchResultRepository repository)
    : IRequestHandler<SaveResultsCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(SaveResultsCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            return ApiResult<bool>.Fail(validatorResult.Errors?.FirstOrDefault()?.ErrorMessage);
        }

        try
        {
            var res = await repository.AddResultAsync(request.Rankings, request.KeyWord, request.Engine, cancellationToken);
            return res ? ApiResult<bool>.Success(true) : ApiResult<bool>.Fail("Failed to save results");
        }
        catch (Exception e)
        {
            return ApiResult<bool>.Fail(e.Message);
        }
    }
}