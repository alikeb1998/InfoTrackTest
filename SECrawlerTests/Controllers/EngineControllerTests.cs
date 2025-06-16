using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SECrawler.Api.Controllers;
using SECrawler.Application.Commands.Search;
using SECrawler.Application.Queries.Engines;
using SECrawler.Domain;
using SECrawler.Domain.Entities;

namespace SECrawlerTests.Controllers;

[TestFixture]
public class EngineControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private EngineController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new EngineController(_mediatorMock.Object);
    }

    [Test]
    public async Task SearchEngines_ReturnsOk_WithEngineList()
    {
        // Arrange
        var engines = new List<Engine>
            { new Engine { Id = 1, BaseUrl = "url", SearchUrl = "search", Expression = "exp" } };
        var expectedResult = ApiResult<List<Engine>>.Success(engines);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<EngineListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.SearchEngines();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Search_ReturnsOk_WithRankings()
    {
        // Arrange
        var searchCommand = new SearchCommand
        {
            Query = "test",
            EngineId = 1,
            PageSize = 10
        };

        var rankings = new List<int> { 1, 5, 10 };
        var expectedResult = ApiResult<List<int>>.Success(rankings);

        _mediatorMock
            .Setup(m => m.Send(searchCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Search(searchCommand);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedResult));
    }
}