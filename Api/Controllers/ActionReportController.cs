using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionReportController : ControllerBase
{
    private readonly RedisConnectionProvider _provider;
    private readonly ActionReportFilter _reportFilter;

    public ActionReportController(RedisConnectionProvider provider, ActionReportFilter reportFilter)
    {
        _provider = provider;
        _reportFilter = reportFilter;
    }

    [HttpGet]
    public IActionResult GetStatistics()
    {
        return Ok(_reportFilter.GetStatistics());
    }
}
