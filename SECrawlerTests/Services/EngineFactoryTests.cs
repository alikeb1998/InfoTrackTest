using Moq;
using SECrawler.Application.Services;
using SECrawler.Application.Services.Implementations;

namespace SECrawler.Tests.Services;

[TestFixture]
public class EngineFactoryTests
{
    private Mock<IEngineRepository> _engineRepoMock = null!;
    private Mock<ISearchResultRepository> _searchResultRepoMock = null!;
    private Mock<IHttpClientWrapper> _httpClientMock = null!;
    private EngineFactory _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _engineRepoMock = new Mock<IEngineRepository>();
        _searchResultRepoMock = new Mock<ISearchResultRepository>();
        _httpClientMock = new Mock<IHttpClientWrapper>();

        _factory = new EngineFactory(_engineRepoMock.Object, _searchResultRepoMock.Object, _httpClientMock.Object);
    }

    [TestCase(1, typeof(GoogleEngineService))]
    [TestCase(2, typeof(BingEngineService))]
    public void CreateEngineService_ShouldReturnCorrectServiceInstance(int engineId, Type expectedType)
    {
        // Act
        var service = _factory.CreateEngineService(engineId);

        // Assert
        Assert.That(service, Is.Not.Null);
        Assert.That(service.GetType(), Is.EqualTo(expectedType));
    }

    [Test]
    public void CreateEngineService_ShouldThrowException_WhenEngineIdIsInvalid()
    {
        // Arrange
        var invalidId = 999;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _factory.CreateEngineService(invalidId));
        Assert.That(ex?.Message, Is.EqualTo("Invalid engine type"));
    }
}