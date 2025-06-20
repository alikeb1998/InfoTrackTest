﻿using Microsoft.EntityFrameworkCore;
using SECrawler.Domain.Entities;

namespace SECrawler.Infrastructure.Data;

public class EfDbContext : DbContext
{
    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
    }

      public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: SearchResult, State: EntityState.Added });
        foreach (var entry in entries)
        {
            ((SearchResult)entry.Entity).Date = DateTime.Now;
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SE");
        modelBuilder.ApplyConfiguration(new Configurations.EnginesConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SearchResultConfiguration());
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Engine>().HasData(
            new List<Engine>()
            {
                new()
                {
                    Id = 1,
                    BaseUrl = "http://www.google.co.uk",
                    Expression = @"gMi0 kCrYT(.+?)sa=U&ved=",
                    SearchUrl = "search?q=#query#&num=#pageSize#",
                    Name = "Google"
                },
                new()
                {
                    Id = 2,
                    BaseUrl = "http://www.bing.com",
                    Expression = @"((<cite>)(.+?)(</cite>))",
                    SearchUrl = "search?q=#query#&count=#pageSize#",
                    Name = "Bing"
                },
            }
        );
    }

    public virtual DbSet<SearchResult> SearchResults { get; set; }
    public virtual DbSet<Engine> Engines { get; set; }
}