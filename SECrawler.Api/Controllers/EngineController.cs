using MediatR;
using Microsoft.AspNetCore.Mvc;
using SECrawler.Application.Commands.Search;
using SECrawler.Application.Queries.Engines;

namespace SECrawler.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EngineController : ControllerBase
{
    private readonly IMediator Mediator;

    public EngineController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchCommand request)
    {

        var result = await Mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("SearchEngines")]
    public async Task<IActionResult> SearchEngines()
    {
        var result = await Mediator.Send(new EngineListQuery());
        return Ok(result);
    }
}