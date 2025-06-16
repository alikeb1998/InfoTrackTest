using System.Collections;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SECrawler.Application.Commands.Results;
using SECrawler.Application.Services;
using SECrawler.Domain;

namespace SECrawlerTests.Commands;


[TestFixture]
public class SaveResultsCommandHandlerTests
{
    private Mock<IValidator<SaveResultsCommand>> _validatorMock = null!;
    private Mock<ISearchResultRepository> _repositoryMock = null!;
    private SaveResultsCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _validatorMock = new Mock<IValidator<SaveResultsCommand>>();
        _repositoryMock = new Mock<ISearchResultRepository>();
        _handler = new SaveResultsCommandHandler(_validatorMock.Object, _repositoryMock.Object);
    }

    [TestCase(EngineType.Google, "", TestName = "Handle_ShouldFail_WhenKeywordIsEmpty")]
    [TestCase(0, "keyword", TestName = "Handle_ShouldFail_WhenEngineIsEmpty")]
    public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid(EngineType engine, string keyword)
    {
        // Arrange
        var command = new SaveResultsCommand { KeyWord = keyword, Engine = engine, Rankings = new List<int>() };

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Field", "Validation failed")
        });

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Validation failed"));
    }

    [TestCaseSource(nameof(RepositorySaveScenarios))]
    public async Task Handle_ShouldBehaveAsExpected_BasedOnRepositoryResult(bool repoResult, bool expectedSuccess, string expectedError)
    {
        // Arrange
        var command = new SaveResultsCommand { KeyWord = "test", Engine = EngineType.Google, Rankings = new List<int> { 1, 2 } };
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.AddResultAsync(command.Rankings, command.KeyWord, command.Engine, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(repoResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
        Assert.That(result.ErrorMessage, Is.EqualTo(expectedError));
    }

    private static IEnumerable RepositorySaveScenarios
    {
        get
        {
            yield return new TestCaseData(false, false, "Failed to save results").SetName("Handle_ShouldFail_WhenRepositoryReturnsFalse");
            yield return new TestCaseData(true, true, null).SetName("Handle_ShouldSucceed_WhenRepositoryReturnsTrue");
        }
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        var command = new SaveResultsCommand { KeyWord = "test", Engine = EngineType.Google, Rankings = new List<int> { 1, 2 } };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.AddResultAsync(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<EngineType>(), It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("boom"));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("boom"));
    }
}