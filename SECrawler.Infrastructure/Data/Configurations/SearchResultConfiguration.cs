using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SECrawler.Domain.Entities;

namespace SECrawler.Infrastructure.Data.Configurations;

public class SearchResultConfiguration: IEntityTypeConfiguration<SearchResult>
{
    public void Configure(EntityTypeBuilder<SearchResult> builder)
    {
        builder.ToTable("SearchResults");
        builder.HasKey(e => e.Id);
        
        builder.Property(x => x.Rank)
            .IsRequired();
        builder.Property(x => x.EngineType)
            .IsRequired();
        builder.Property(x => x.KeyWords)
            .IsRequired();
    }
}