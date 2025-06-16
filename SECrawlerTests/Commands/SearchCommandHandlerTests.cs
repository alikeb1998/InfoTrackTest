using System.Collections;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SECrawler.Application.Commands.Search;
using SECrawler.Application.Services;

namespace SECrawlerTests.Commands;

[TestFixture]
public class SearchCommandHandlerTests
{
    private Mock<IValidator<SearchCommand>> _validatorMock = null!;
    private Mock<IEngineFactory> _engineFactoryMock = null!;
    private Mock<IEngineService> _engineServiceMock = null!;
    private SearchCommandHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _validatorMock = new Mock<IValidator<SearchCommand>>();
        _engineFactoryMock = new Mock<IEngineFactory>();
        _engineServiceMock = new Mock<IEngineService>();

        _handler = new SearchCommandHandler(_validatorMock.Object, _engineFactoryMock.Object);
    }

    [TestCase(null, 1, 10, TestName = "Handle_ShouldFail_WhenQueryIsNull")]
    [TestCase("", 1, 10, TestName = "Handle_ShouldFail_WhenQueryIsEmpty")]
    public async Task Handle_ShouldReturnValidationFailure_WhenCommandIsInvalid(string? query, int engineId, int pageSize)
    {
        // Arrange
        var command = new SearchCommand { Query = query, EngineId = engineId, PageSize = pageSize };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult(new[]
                      {
                          new ValidationFailure("Query", "Query is required")
                      }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Query is required"));
    }

    [TestCaseSource(nameof(SearchResultTestCases))]
    public async Task Handle_ShouldReturnExpectedResult_BasedOnEngineServiceResult(List<int> rankings, bool expectedSuccess, string expectedError)
    {
        var command = new SearchCommand { Query = "infotrack", EngineId = 1, PageSize = 10 };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _engineFactoryMock.Setup(f => f.CreateEngineService(command.EngineId))
                          .Returns(_engineServiceMock.Object);

        _engineServiceMock.Setup(s => s.GetRankingsAsync(command.Query, command.EngineId, command.PageSize))
                          .ReturnsAsync(rankings);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
        Assert.That(result.Data, Is.EqualTo(rankings));
        Assert.That(result.ErrorMessage, Is.EqualTo(expectedError));
    }

    private static IEnumerable SearchResultTestCases
    {
        get
        {
            yield return new TestCaseData(new List<int> { 0, 2 }, true, null).SetName("Handle_ShouldReturnSuccess_WhenValid");
            yield return new TestCaseData(new List<int>(), true, null).SetName("Handle_ShouldReturnSuccess_WhenEmptyRankingList");
        }
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        var command = new SearchCommand { Query = "infotrack", EngineId = 2, PageSize = 10 };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _engineFactoryMock.Setup(f => f.CreateEngineService(command.EngineId))
                          .Returns(_engineServiceMock.Object);

        _engineServiceMock.Setup(s => s.GetRankingsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                          .ThrowsAsync(new Exception("Something went wrong"));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Something went wrong"));
    }
}