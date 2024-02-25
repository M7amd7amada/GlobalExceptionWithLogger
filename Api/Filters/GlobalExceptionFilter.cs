using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Redis.OM;
using Redis.OM.Searching;
using System.Text;

namespace Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IConfiguration _configuration;
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<ExceptionReportInfo> _exceptionInfo;

    public GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger,
        IConfiguration configuration,
        RedisConnectionProvider provider)
    {
        _logger = logger;
        _configuration = configuration;
        _provider = provider;
        _exceptionInfo = (RedisCollection<ExceptionReportInfo>)
            _provider.RedisCollection<ExceptionReportInfo>();
    }

    public void OnException(ExceptionContext context)
    {
        LogExceptionDetails(context.Exception);

        CreateLogDetails(context);

        // LogToDisk(logDetails);

        context.Result = CreateErrorResponse(context);
        context.ExceptionHandled = true;
    }

    private void LogExceptionDetails(Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception has occurred.");
    }

    private void CreateLogDetails(ExceptionContext context)
    {
        context.HttpContext.Response.OnCompleted(async () =>
        {
            var foo = new ExceptionReportInfo
            {
                ExMessage = context.Exception.Message,
                InnerExMessage = context.Exception.InnerException?.Message!,
                ExceptionType = context.Exception.GetType().FullName!,
                ActionName = context.ActionDescriptor.RouteValues["action"]!,
                ControllerName = context.ActionDescriptor.RouteValues["controller"]!,
                Parameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
                Response = context.HttpContext.Response.StatusCode,
                Created = DateTime.Now
            };
            await LogToRedis(foo);
        });
    }

    // private void LogToDisk(dynamic logDetails)
    // {
    //     var logDirectory = _configuration["LogsDirName"];
    //     var fileName = $"log-{DateTime.Today:yyyy-MM-dd}.txt";
    //     var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory!, fileName);

    //     EnsureLogFileExists(logFilePath);

    //     var logMessage = JsonConvert.SerializeObject(logDetails, Formatting.Indented);

    //     using var streamWriter = new StreamWriter(logFilePath, true, Encoding.UTF8);
    //     streamWriter.WriteLine(logMessage);
    // }

    private async Task LogToRedis(ExceptionReportInfo info)
    {
        await _exceptionInfo.InsertAsync(info);
    }

    // private void EnsureLogFileExists(string filePath)
    // {
    //     if (!File.Exists(filePath))
    //     {
    //         using (File.Create(filePath)) { }
    //     }
    // }

    private ObjectResult CreateErrorResponse(ExceptionContext context)
    {
        return new ObjectResult(new
        {
            Message = "Internal Server Error",
            Details = context.Exception.Message
        })
        {
            StatusCode = 500
        };
    }
}
