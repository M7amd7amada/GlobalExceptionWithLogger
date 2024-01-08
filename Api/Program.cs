using Api.Filters;
using Api.Models;
using Api.Services;
using Redis.OM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
    options.Filters.Add(typeof(GlobalEndpointFilter));
    options.Filters.Add(typeof(ActionReportFilter));
});

builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["ConnectionStrings:RedisConnectionString"]!));
var statistics = new Dictionary<string, ActionReportInfo>();
builder.Services.AddSingleton(statistics);
builder.Services.AddSingleton<ActionReportFilter>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
