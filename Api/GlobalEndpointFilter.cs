using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Api;

public class GlobalEndpointFilter : IActionFilter, IExceptionFilter
{
    private readonly IDatabase _redisDb;

    public GlobalEndpointFilter(
        IConnectionMultiplexer multiplexer)
    {
        _redisDb = multiplexer.GetDatabase();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        LogApiRequestDetails(context);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        LogApiResponseDetails(context);
    }

    public void OnException(ExceptionContext context)
    {
        LogExceptionDetails(context);
    }

    private void LogApiRequestDetails(ActionExecutingContext context)
    {
        var requestDetails = new
        {
            HttpMethod = context.HttpContext.Request.Method,
            Path = context.HttpContext.Request.Path,
            QueryParameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
            Headers = JsonConvert.SerializeObject(context.HttpContext.Request.Headers),
            Created = DateTime.Now
        };

        var logKey = $"{requestDetails.HttpMethod}-{requestDetails.Path}-Request";

        LogToRedis(requestDetails, logKey);
    }

    private void LogApiResponseDetails(ActionExecutedContext context)
    {
        var responseDetails = new
        {
            HttpMethod = context.HttpContext.Request.Method,
            Path = context.HttpContext.Request.Path,
            StatusCode = context.HttpContext.Response.StatusCode,
            QueryParameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
            Headers = JsonConvert.SerializeObject(context.HttpContext.Response.Headers),
            Created = DateTime.Now
        };

        var logKey = $"{responseDetails.HttpMethod}-{responseDetails.Path}-{responseDetails.StatusCode}-{responseDetails.Created}";

        LogToRedis(responseDetails, logKey);
    }

    private void LogExceptionDetails(ExceptionContext context)
    {
        var exceptionDetails = new
        {
            HttpMethod = context.HttpContext.Request.Method,
            Path = context.HttpContext.Request.Path,
            ExceptionType = context.Exception.GetType().FullName,
            ExMessage = context.Exception.Message,
            InnerExMessage = context.Exception.InnerException?.Message,
            ActionName = context.ActionDescriptor.RouteValues["action"],
            ControllerName = context.ActionDescriptor.RouteValues["controller"],
            Parameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
            StatusCode = context.HttpContext.Response.StatusCode,
            Created = DateTime.Now
        };

        var logKey = $"{exceptionDetails.HttpMethod}-{exceptionDetails.Path}-{exceptionDetails.StatusCode}-Error-{exceptionDetails.Created}";

        LogToRedis(exceptionDetails, logKey);
    }

    private void LogToRedis(object? logDetails, string logKey)
    {
        var logMessage = JsonConvert.SerializeObject(logDetails, Formatting.Indented);
        _redisDb.StringSet(logKey, logMessage);
    }
}
