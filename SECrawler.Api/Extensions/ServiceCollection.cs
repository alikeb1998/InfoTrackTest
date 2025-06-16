using Microsoft.EntityFrameworkCore;
using SECrawler.Application.Commands.Results;
using SECrawler.Application.Commands.Search;
using SECrawler.Application.Services;
using SECrawler.Application.Services.Implementations;
using SECrawler.Infrastructure.Data;
using SECrawler.Infrastructure.Repository;

namespace SECrawler.Api.Extensions;
using FluentValidation;

public static class ServiceCollection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("sqlConnection");
        services.AddDbContext<EfDbContext>(options => { options.UseSqlServer(connectionString); });
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SaveResultsCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SearchCommand>());
        services.AddValidatorsFromAssemblyContaining<SaveResultsCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<SearchCommandValidator>();
        
        services.AddScoped<IEngineRepository, EngineRepository>();
        services.AddScoped<ISearchResultRepository, SearchResultRepository>();
        services.AddScoped<IEngineService, GoogleEngineService>();
        services.AddScoped<IEngineService, BingEngineService>();
        
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();

        
        services.AddScoped<IEngineFactory, EngineFactory>();
        
        return services;
    }
}