using Api.Filters;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionReportController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatistics()
    {
        return Ok();
    }
}
