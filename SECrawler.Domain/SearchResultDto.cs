namespace SECrawler.Domain;

public class SearchResultDto
{
    public long Id { get; set; }
    public string EngineType { get; set; }

    public string KeyWords { get; set; }

    public string Rank { get; set; }

    public DateTime Date { get; set; }
}