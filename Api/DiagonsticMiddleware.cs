namespace Api;
public class StatusCodeDiagnosticMiddleware
{
    private readonly RequestDelegate _next;

    public StatusCodeDiagnosticMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalStatusCode = context.Response.StatusCode;
        System.Diagnostics.Debug.WriteLine($"Original Status Code: {originalStatusCode}");

        await _next(context);

        var finalStatusCode = context.Response.StatusCode;
        System.Diagnostics.Debug.WriteLine($"Final Status Code: {finalStatusCode}");
    }
}
