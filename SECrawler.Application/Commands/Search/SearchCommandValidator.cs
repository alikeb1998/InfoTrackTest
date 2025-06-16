using FluentValidation;

namespace SECrawler.Application.Commands.Search;

public class SearchCommandValidator: AbstractValidator<SearchCommand>
{
    public SearchCommandValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty().WithMessage("Query is required.")
            .MaximumLength(50).WithMessage("Query is too long.");
    }
}