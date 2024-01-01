var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
