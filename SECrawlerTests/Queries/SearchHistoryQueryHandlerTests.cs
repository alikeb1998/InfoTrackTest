using Moq;
using SECrawler.Application.Queries.SearchHistory;
using SECrawler.Application.Services;
using SECrawler.Domain.Entities;

namespace SECrawlerTests.Queries;

  [TestFixture]
    public class SearchHistoryQueryHandlerTests
    {
        private Mock<ISearchResultRepository> _repositoryMock;
        private SearchHistoryQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ISearchResultRepository>();
            _handler = new SearchHistoryQueryHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSearchResultsSuccessfully()
        {
            // Arrange
            var mockData = new List<SearchResult>
            {
                new SearchResult
                {
                    Id = 1,
                    Date = new DateTime(2024, 1, 1),
                    Rank = "1",
                    EngineType = "Google",
                    KeyWords = "unit testing"
                },
                new SearchResult
                {
                    Id = 2,
                    Date = new DateTime(2024, 2, 1),
                    Rank = "3",
                    EngineType = "Bing",
                    KeyWords = "integration testing"
                }
            };

            _repositoryMock.Setup(r => r.GetHistoriesAsync()).ReturnsAsync(mockData);

            var query = new SearchHistoryQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(2));

            Assert.That(result.Data.Any(x => x.EngineType == "Google" && x.KeyWords == "unit testing"));
            Assert.That(result.Data.Any(x => x.EngineType == "Bing" && x.KeyWords == "integration testing"));
        }

        [Test]
        public async Task Handle_ReturnsEmptyList_WhenNoHistoryExists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetHistoriesAsync()).ReturnsAsync(new List<SearchResult>());

            var query = new SearchHistoryQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
            Assert.That(result.Data, Is.Empty);
        }
    }