using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "An unhandled exception has occurred.");

        var logDetails = new
        {
            ExMessage = context.Exception.Message,
            InnerExMessage = context.Exception.InnerException?.Message,
            ActionName = context.ActionDescriptor.RouteValues["action"],
            ControllerName = context.ActionDescriptor.RouteValues["controller"],
            Parameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
            Response = context.HttpContext.Response.StatusCode,
            Created = DateTime.Now
        };

        logger.LogInformation($"Error Details: {logDetails}");

        context.Result = new ObjectResult(new
        {
            Message = "Internal Server Error",
            Details = context.Exception.Message
        })
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}
