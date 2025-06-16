using FluentValidation;

namespace SECrawler.Application.Commands.Results;

public class SaveResultsCommandValidator: AbstractValidator<SaveResultsCommand>
{
    public SaveResultsCommandValidator()
    {
        RuleFor(x => x.KeyWord)
            .NotEmpty().WithMessage("Search phrase is required.")
            .MaximumLength(200).WithMessage("Search phrase is too long.");
    }
}