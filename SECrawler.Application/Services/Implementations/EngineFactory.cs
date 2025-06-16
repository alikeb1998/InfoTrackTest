namespace SECrawler.Application.Services.Implementations;

public class EngineFactory(IEngineRepository dataService, ISearchResultRepository searchResultDataService, IHttpClientWrapper wrapper) : IEngineFactory
{
    public IEngineService CreateEngineService( int engineId)
    {
        switch (engineId)
        {
            case 1:
                return new GoogleEngineService(dataService, wrapper);
            case 2:
                return new BingEngineService(dataService, wrapper);
            default:
                throw new ArgumentException("Invalid engine type");
        }
    }
}