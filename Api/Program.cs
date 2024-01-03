using Api.Filters;
using Api.Services;
using Redis.OM;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
    options.Filters.Add(typeof(GlobalEndpointFilter));
});

builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["ConnectionStrings:RedisConnectionString"]!));




var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
