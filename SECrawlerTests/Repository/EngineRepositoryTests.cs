using Microsoft.EntityFrameworkCore;
using SECrawler.Domain.Entities;
using SECrawler.Infrastructure.Data;
using SECrawler.Infrastructure.Repository;

namespace SECrawlerTests.Repository;

[TestFixture]
public class EngineRepositoryTests
{
    private EfDbContext _context = null!;
    private EngineRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<EfDbContext>()
            .UseInMemoryDatabase(databaseName: $"InMemoryDb-{Guid.NewGuid()}")
            .Options;

        _context = new EfDbContext(options);
        
        _context.Engines.AddRange(
            new Engine { Id = 1, Name = "Google", BaseUrl = "https://google.com", SearchUrl = "search?q=#query#", Expression = "regex1" },
            new Engine { Id = 2, Name = "Bing", BaseUrl = "https://bing.com", SearchUrl = "search?q=#query#", Expression = "regex2" }
        );
        _context.SaveChanges();

        _repository = new EngineRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetEnginesAsync_ReturnsAllEngines()
    {
        var result = await _repository.GetEnginesAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Any(e => e.Name == "Google"));
        Assert.That(result.Any(e => e.Name == "Bing"));
    }

    [Test]
    public async Task GetOneAsync_ReturnsCorrectEngine_WhenIdExists()
    {
        var result = await _repository.GetOneAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Google"));
    }

    [Test]
    public async Task GetOneAsync_ReturnsNull_WhenIdDoesNotExist()
    {
        var result = await _repository.GetOneAsync(999);

        Assert.That(result, Is.Null);
    }
}