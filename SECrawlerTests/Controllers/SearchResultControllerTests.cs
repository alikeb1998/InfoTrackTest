using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SECrawler.Api.Controllers;
using SECrawler.Application.Commands.Results;
using SECrawler.Application.Queries.SearchHistory;
using SECrawler.Domain;

namespace SECrawlerTests.Controllers;

[TestFixture]
public class SearchResultControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private SearchResultController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new SearchResultController(_mediatorMock.Object);
    }

    [Test]
    public async Task Histories_ReturnsOk_WithSearchResults()
    {
        // Arrange
        var results = new List<SearchResultDto>
        {
            new SearchResultDto { Id = 1, KeyWords = "test", EngineType = "Google", Rank = "1,2", Date = DateTime.Now }
        };
        var expected = ApiResult<List<SearchResultDto>>.Success(results);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.Histories();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task SaveSearchHistory_ReturnsOk_WithResult()
    {
        // Arrange
        var command = new SaveResultsCommand
        {
            KeyWord = "infotrack",
            Engine = EngineType.Google,
            Rankings = new List<int> { 1, 3, 5 }
        };

        var expected = ApiResult<bool>.Success(true);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.SaveSearchHistory(command);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expected));
    }
}

