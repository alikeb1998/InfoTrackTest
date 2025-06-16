using Microsoft.EntityFrameworkCore;
using SECrawler.Domain;
using SECrawler.Domain.Entities;
using SECrawler.Infrastructure.Data;
using SECrawler.Infrastructure.Repository;

namespace SECrawlerTests.Repository;

[TestFixture]
public class SearchResultRepositoryTests
{
    private EfDbContext _dbContext = null!;
    private SearchResultRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EfDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new EfDbContext(options);
        _repository = new SearchResultRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task GetHistoriesAsync_ReturnsSavedResults()
    {
        // Arrange
        var result = new SearchResult
        {
            EngineType = "Google",
            KeyWords = "test keyword",
            Date = DateTime.UtcNow,
            Rank = "1,2,3"
        };

        _dbContext.SearchResults.Add(result);
        await _dbContext.SaveChangesAsync();

        // Act
        var results = await _repository.GetHistoriesAsync();

        // Assert
        Assert.That(results, Has.Count.EqualTo(1));
        Assert.That(results[0].KeyWords, Is.EqualTo("test keyword"));
    }

    [Test]
    public async Task AddResultAsync_SavesResultSuccessfully()
    {
        // Arrange
        var rankings = new List<int> { 1, 2, 3 };
        var keywords = "seo";
        var engineType = EngineType.Google;

        // Act
        var success = await _repository.AddResultAsync(rankings, keywords, engineType, CancellationToken.None);

        var saved = await _dbContext.SearchResults.FirstOrDefaultAsync();

        // Assert
        Assert.IsTrue(success);
        Assert.IsNotNull(saved);
        Assert.That(saved?.KeyWords, Is.EqualTo(keywords));
        Assert.That(saved?.Rank, Is.EqualTo("1,2,3"));
        Assert.That(saved?.EngineType, Is.EqualTo("Google"));
    }
}