using Moq;
using SECrawler.Application.Queries.Engines;
using SECrawler.Application.Services;
using SECrawler.Domain.Entities;

namespace SECrawlerTests.Queries;

[TestFixture]
public class EngineListHandlerTests
{
    private Mock<IEngineRepository> _repositoryMock;
    private EngineListHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IEngineRepository>();
        _handler = new EngineListHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessResult_WithListOfEngines()
    {
        // Arrange
        var engines = new List<Engine>
        {
            new Engine { Id = 1, Name = "Google" },
            new Engine { Id = 2, Name = "Bing" }
        };

        _repositoryMock.Setup(r => r.GetEnginesAsync()).ReturnsAsync(engines);

        var query = new EngineListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Count, Is.EqualTo(2));
        Assert.That(result.Data.Any(e => e.Name == "Google"), Is.True);
        Assert.That(result.Data.Any(e => e.Name == "Bing"), Is.True);
    }

    [Test]
    public async Task Handle_ReturnsSuccessResult_WithEmptyList_WhenNoEnginesFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetEnginesAsync()).ReturnsAsync(new List<Engine>());

        var query = new EngineListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data, Is.Empty);
    }
}
