using Api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace Api.Filters;

public class ActionReportFilter : IActionFilter
{
    private readonly Dictionary<string, ActionReportInfo> _statistics;

    public ActionReportFilter(Dictionary<string, ActionReportInfo> statistics)
    {
        _statistics = statistics;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        IncrementCallCount(controller!, action!);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        IncrementStatusCodeCount(controller!, action!, context.HttpContext.Response.StatusCode);
    }

    private void IncrementCallCount(string controller, string action)
    {
        var key = $"{controller}Controller.{action}";
        if (!_statistics.ContainsKey(key))
        {
            _statistics[key] = new ActionReportInfo { CallCount = 0, StatusCodesCount = new Dictionary<int, int>() };
        }

        _statistics[key].CallCount++;
    }

    private void IncrementStatusCodeCount(string controller, string action, int statusCode)
    {
        var key = $"{controller}Controller.{action}";
        if (!_statistics.ContainsKey(key))
        {
            _statistics[key] = new ActionReportInfo { CallCount = 0, StatusCodesCount = new Dictionary<int, int>() };
        }

        if (!_statistics[key].StatusCodesCount!.ContainsKey(statusCode))
        {
            _statistics[key].StatusCodesCount![statusCode] = 0;
        }

        _statistics[key].StatusCodesCount![statusCode]++;
    }
    public Dictionary<string, ActionReportInfo> GetStatistics()
    {
        return _statistics;
    }
}
