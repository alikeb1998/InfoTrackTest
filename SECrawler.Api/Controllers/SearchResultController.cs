using MediatR;
using Microsoft.AspNetCore.Mvc;
using SECrawler.Application.Commands.Results;
using SECrawler.Application.Queries.SearchHistory;

namespace SECrawler.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchResultController(IMediator Mediator) : ControllerBase
{
    
    [HttpGet("Histories")]
    public async Task<IActionResult> Histories()
    {
        var result = await Mediator.Send(new SearchHistoryQuery());
        return Ok(result);
    }

    [HttpPost("SaveSearchHistory")]
    public async Task<IActionResult> SaveSearchHistory([FromBody] SaveResultsCommand historyQuery)
    {
        var result = await Mediator.Send(historyQuery);
        return Ok(result);
    }
    
}