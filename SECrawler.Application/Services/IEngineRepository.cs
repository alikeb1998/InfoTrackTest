using SECrawler.Domain.Entities;

namespace SECrawler.Application.Services;

public interface IEngineRepository
{
    Task<IEnumerable<Engine>> GetEnginesAsync();

    Task<Engine?> GetOneAsync(int id);
}