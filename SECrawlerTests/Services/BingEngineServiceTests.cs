using Moq;
using SECrawler.Application.Services;
using SECrawler.Application.Services.Implementations;
using SECrawler.Domain.Entities;

namespace SECrawler.Tests.Services;

[TestFixture]
public class BingEngineServiceTests
{
    private Mock<IEngineRepository> _engineRepoMock;
    private Mock<IHttpClientWrapper> _httpClientMock;
    private BingEngineService _service;

    [SetUp]
    public void SetUp()
    {
        _engineRepoMock = new Mock<IEngineRepository>();
        _httpClientMock = new Mock<IHttpClientWrapper>();
        _service = new BingEngineService(_engineRepoMock.Object, _httpClientMock.Object);
    }

    [Test]
    public async Task GetRankingsAsync_ReturnsEmptyList_WhenEngineIsNotFound()
    {
        _engineRepoMock.Setup(x => x.GetOneAsync(It.IsAny<int>())).ReturnsAsync((Engine)null!);

        var result = await _service.GetRankingsAsync("test", 1, 10);

        Assert.That(result, Is.Empty);
    }

    [TestCaseSource(nameof(GetRankingTestCases))]
    public async Task GetRankingsAsync_ReturnsExpectedRanks(string html, string expression, List<int> expectedRanks)
    {
        var engine = new Engine
        {
            Id = 1,
            BaseUrl = "https://bing.com",
            SearchUrl = "search?q=#query#&count=#pageSize#",
            Expression = expression
        };

        _engineRepoMock.Setup(x => x.GetOneAsync(1)).ReturnsAsync(engine);
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(html);

        var result = await _service.GetRankingsAsync("infotrack", 1, 10);

        Assert.That(result, Is.EquivalentTo(expectedRanks));
    }

    public static IEnumerable<TestCaseData> GetRankingTestCases()
    {
        yield return new TestCaseData(
            "<a href=\"https://www.infotrack.co.uk/page1\">link1</a>" +
            "<a href=\"https://other.com\">link2</a>" +
            "<a href=\"https://www.infotrack.co.uk/page2\">link3</a>",
            "<a href=\"(.*?)\">",
            new List<int> { 0, 2 }
        ).SetName("Multiple infotrack links at index 0 and 2");

        yield return new TestCaseData(
            "&lt;a href=\"https://www.infotrack.co.uk\"&gt;",
            "<a href=\"(.*?)\">",
            new List<int> { 0 }
        ).SetName("Encoded HTML gets decoded and matched");

        yield return new TestCaseData(
            "<a href=\"https://other.com\">Other</a>",
            "<a href=\"(.*?)\">",
            new List<int>()
        ).SetName("No infotrack links returns empty list");
    }
}