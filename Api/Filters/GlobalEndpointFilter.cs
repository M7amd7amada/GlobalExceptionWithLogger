using Api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Redis.OM;
using Redis.OM.Searching;

namespace Api.Filters
{
    public class GlobalEndpointFilter : IActionFilter, IExceptionFilter
    {
        private readonly RedisConnectionProvider _provider;
        private readonly RedisCollection<LogApiRequestDetails> _requestLog;

        public GlobalEndpointFilter(RedisConnectionProvider provider)
        {
            _provider = provider;
            _requestLog = (RedisCollection<LogApiRequestDetails>)provider.RedisCollection<LogApiRequestDetails>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do Nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestDetails = CreateApiRequestLog(context, null);
            LogToRedis(requestDetails);
        }

        public void OnException(ExceptionContext context)
        {
            var requestDetails = CreateApiRequestLog(context, context.Exception);
            LogToRedis(requestDetails);
        }

        private LogApiRequestDetails CreateApiRequestLog(FilterContext context, Exception? exception)
        {
            return new LogApiRequestDetails
            {
                HttpMethod = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
                QueryParameters = JsonConvert.SerializeObject(context.HttpContext.Request.Query),
                StatusCode = context.HttpContext.Response.StatusCode,
                ActionName = context.ActionDescriptor.RouteValues["action"]!,
                ControllerName = context.ActionDescriptor.RouteValues["controller"]!,
                ExceptionType = (exception != null ? exception?.GetType().FullName : string.Empty)!,
                Created = DateTime.Now
            };
        }

        private async void LogToRedis(LogApiRequestDetails logDetails)
        {
            await _requestLog.InsertAsync(logDetails);
        }
    }
}
