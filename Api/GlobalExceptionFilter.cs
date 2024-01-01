using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> logger;
    private readonly IConfiguration configuration;

    public GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
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

        var logDirectory = configuration["LogsDirName"];

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory!);
        }

        var fileName = $"log-{DateTime.Today:yyyy-MM-dd}.txt";
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory!, fileName);

        EnsureLogFileExists(logFilePath);
        LogToFile(logFilePath, logDetails);

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

    private static void EnsureLogFileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            using var fileStream = File.Create(filePath);
        }
    }

    private static void LogToFile(string filePath, object logDetails)
    {
        var logMessage = JsonConvert.SerializeObject(logDetails, Formatting.Indented);

        using var streamWriter = new StreamWriter(filePath, true, Encoding.UTF8);
        streamWriter.WriteLine(logMessage);
    }
}
