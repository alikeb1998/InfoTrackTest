using Moq;
using SECrawler.Application.Services;
using SECrawler.Application.Services.Implementations;
using SECrawler.Domain.Entities;

namespace SECrawler.Tests.Services;

[TestFixture]
public class GoogleEngineServiceTests
{
    private Mock<IEngineRepository> _engineRepoMock = null!;
    private Mock<IHttpClientWrapper> _httpClientMock = null!;
    private GoogleEngineService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _engineRepoMock = new Mock<IEngineRepository>();
        _httpClientMock = new Mock<IHttpClientWrapper>();
        _service = new GoogleEngineService(_engineRepoMock.Object, _httpClientMock.Object);
    }

    [Test]
    public async Task GetRankingsAsync_ReturnsEmptyList_WhenEngineIsNotFound()
    {
        _engineRepoMock.Setup(repo => repo.GetOneAsync(It.IsAny<int>()))
                       .ReturnsAsync((Engine?)null);

        var result = await _service.GetRankingsAsync("test", 999, 10);

        Assert.That(result, Is.Empty);
    }

    private static IEnumerable<TestCaseData> RankingTestCases()
    {
        yield return new TestCaseData(
            new Engine
            {
                Id = 1,
                BaseUrl = "https://google.com",
                SearchUrl = "search?q=#query#&num=#pageSize#",
                Expression = "<a href=\"(.*?)\">"
            },
            "<a href=\"https://www.infotrack.co.uk/page1\">link1</a>" +
            "<a href=\"https://other.com\">link2</a>" +
            "<a href=\"https://www.infotrack.co.uk/page2\">link3</a>",
            new List<int> { 0, 2 }
        ).SetName("ReturnsCorrectRanks_WithValidLinks");

        yield return new TestCaseData(
            new Engine
            {
                Id = 2,
                BaseUrl = "https://google.com",
                SearchUrl = "search?q=#query#&num=#pageSize#",
                Expression = "<a href=\"(.*?)\">"
            },
            "&lt;a href=\"https://www.infotrack.co.uk/page99\"&gt;link&lt;/a&gt;",
            new List<int> { 0 }
        ).SetName("HandlesHtmlEntities_AndExtractsCorrectRanks");
    }

    [TestCaseSource(nameof(RankingTestCases))]
    public async Task GetRankingsAsync_ReturnsExpectedRanks(Engine engine, string htmlResponse, List<int> expectedRanks)
    {
        _engineRepoMock.Setup(x => x.GetOneAsync(engine.Id)).ReturnsAsync(engine);
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                       .ReturnsAsync(htmlResponse);

        var result = await _service.GetRankingsAsync("infotrack", engine.Id, 10);

        Assert.That(result, Is.EqualTo(expectedRanks));
    }
}