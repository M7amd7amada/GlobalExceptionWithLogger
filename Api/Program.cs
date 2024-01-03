using Api;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
    options.Filters.Add(typeof(GlobalEndpointFilter));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
    .Connect(builder.Configuration["ConnectionStrings:RedisConnectionString"]!));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
