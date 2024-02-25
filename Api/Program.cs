using Api;
using Api.Filters;
using Api.Models;
using Api.Services;
using Redis.OM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ActionReportFilter));
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["ConnectionStrings:RedisConnectionString"]!));
var statistics = new Dictionary<string, ActionReportInfo>();
builder.Services.AddSingleton(statistics);
builder.Services.AddSingleton<ActionReportFilter>();
builder.Services.AddSingleton<ExceptionReportInfo>();

var app = builder.Build();

app.UseMiddleware<StatusCodeDiagnosticMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
