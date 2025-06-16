namespace SECrawler.Application.Services;

public interface IEngineFactory
{
    public IEngineService CreateEngineService(int engineId); 
}