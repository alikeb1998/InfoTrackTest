using Microsoft.EntityFrameworkCore;
using SECrawler.Application.Services;
using SECrawler.Domain.Entities;
using SECrawler.Infrastructure.Data;

namespace SECrawler.Infrastructure.Repository;

public class EngineRepository(EfDbContext dbContext) : IEngineRepository
{
    public async Task<IEnumerable<Engine>> GetEnginesAsync()
    {
        var searchEngines = await dbContext.Engines.ToListAsync();
        return searchEngines;
    }

    public async Task<Engine?> GetOneAsync(int id)
    {
        var searchEngine = await dbContext.Engines.FirstOrDefaultAsync(x => x.Id == id);
        return searchEngine;
    }
}