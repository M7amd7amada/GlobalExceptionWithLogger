using Api.Models;
using Microsoft.AspNetCore.Http.Features;
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

    public async void OnException(ExceptionContext context)
    {
        LogExceptionDetails(context.Exception);

        var logDetails = await CreateLogDetails(context);

        LogToDisk(logDetails);

        context.Result = CreateErrorResponse(context);
        context.ExceptionHandled = true;
    }

    private void LogExceptionDetails(Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception has occurred.");
    }

    private async Task<ExceptionReportInfo> CreateLogDetails(ExceptionContext context)
    {
        var info = new ExceptionReportInfo
        {
            ExMessage = context.Exception.Message,
            InnerExMessage = context.Exception.InnerException?.Message!,
            ExceptionType = context.Exception.GetType().FullName!,
            ActionName = context.ActionDescriptor.RouteValues["action"]!,
            ControllerName = context.ActionDescriptor.RouteValues["controller"]!,
            Parameters = GetQueryStringAsString(context.HttpContext.Request),
            Created = DateTime.Now,
            Body = await ReadRequestBody(context.HttpContext.Request)
        };
        context.HttpContext.Response.OnCompleted(async () =>
                {
                    info.Response = context.HttpContext.Response.StatusCode;
                    await LogToRedis(info);
                });
        return info;
    }

    private static string GetQueryStringAsString(HttpRequest request)
    {
        var queryParameters = new StringBuilder();

        var queryString = request.QueryString;
        foreach (var queryParam in queryString.Value!.Split('&'))
        {
            queryParameters.Append(queryParam).Append(' ');
        }

        return queryParameters.ToString();
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        return await reader.ReadToEndAsync();
    }

    private void LogToDisk(dynamic logDetails)
    {
        var logDirectory = _configuration["LogsDirName"];
        var fileName = $"log-{DateTime.Today:yyyy-MM-dd}.txt";
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory!, fileName);

        EnsureLogFileExists(logFilePath);

        var logMessage = JsonConvert.SerializeObject(logDetails, Formatting.Indented);

        using var streamWriter = new StreamWriter(logFilePath, true, Encoding.UTF8);
        streamWriter.WriteLine(logMessage);
    }

    private async Task LogToRedis(ExceptionReportInfo info)
    {
        await _exceptionInfo.InsertAsync(info);
    }

    private static void EnsureLogFileExists(string filePath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        }
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { }
        }
    }

    private static ObjectResult CreateErrorResponse(ExceptionContext context)
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
