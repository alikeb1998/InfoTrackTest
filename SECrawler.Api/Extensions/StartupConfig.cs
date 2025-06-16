using Microsoft.EntityFrameworkCore;
using SECrawler.Infrastructure.Data;

namespace SECrawler.Api.Extensions;

public static class StartupConfig
{
    public static void ApplyDbMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Applying database migrations...");
            var dbContext = services.GetRequiredService<EfDbContext>();
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations done!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "There was an error applying database migrations.");
            throw;
        }
    }
}