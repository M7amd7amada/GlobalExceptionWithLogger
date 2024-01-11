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
    private readonly ActionReportFilter _reportFilter;
    private readonly RedisCollection<LogApiRequestDetails> _actionInfoLogApiRequestDetails;

    public ActionReportController(RedisConnectionProvider provider, ActionReportFilter reportFilter)
    {
        _provider = provider;
        _reportFilter = reportFilter;
        _actionInfoLogApiRequestDetails = (RedisCollection<LogApiRequestDetails>)
            _provider.RedisCollection<LogApiRequestDetails>();
    }

    [HttpGet]
    public IActionResult GetStatistics()
    {
        return Ok(_reportFilter.GetStatistics());
    }
}
