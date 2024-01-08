using Api.Filters;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;
using Redis.OM.Searching;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionReportController : ControllerBase
{
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<ActionReportInfoWrapper> _actionInfo;
    private readonly ActionReportFilter _reportFilter;

    public ActionReportController(RedisConnectionProvider provider, ActionReportFilter reportFilter)
    {
        _provider = provider;
        _actionInfo = (RedisCollection<ActionReportInfoWrapper>)
            _provider.RedisCollection<ActionReportInfoWrapper>();
        _reportFilter = reportFilter;
    }

    [HttpGet]
    public IActionResult GetStatistics()
    {
        return Ok(_reportFilter.GetStatistics());
    }
}
