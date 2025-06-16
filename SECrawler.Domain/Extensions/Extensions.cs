using System.Text.RegularExpressions;

namespace SECrawler.Domain.Extensions;

public class Extensions
{
    /// <summary>
    /// Extracts all matching links from the given HTML response body using the specified regular expression.
    /// </summary>
    /// <param name="responseBody">The HTML or text content from which to extract links.</param>
    /// <param name="regexToExtractLinks">A regular expression pattern used to identify links in the response body.</param>
    /// <returns>A list of strings containing all matched links found in the response.</returns>

    public static List<string> RetrieveLinksFromResponse(string responseBody, string regexToExtractLinks)
    {
        var matches = Regex.Matches(responseBody, $"{regexToExtractLinks}");
        return matches.Select(x => x.Value).ToList();
    }
}