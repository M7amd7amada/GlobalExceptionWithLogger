using Api.Filters;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionReportController : ControllerBase
{
    private readonly ActionReportFilter _requestStatisticsActionFilter;

    public ActionReportController(ActionReportFilter requestStatisticsActionFilter)
    {
        _requestStatisticsActionFilter = requestStatisticsActionFilter;
    }

    [HttpGet]
    public ActionResult<Dictionary<string, ActionReportInfo>> GetStatistics()
    {
        return Ok(_requestStatisticsActionFilter.GetStatistics());
    }
}
