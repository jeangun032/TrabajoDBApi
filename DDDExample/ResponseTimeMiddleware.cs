using System.Diagnostics;
using System.Threading.Tasks;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace DDDExample.API.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IResponseTimeLogRepository _repository;
        private const int SlowRequestThresholdMs = 1000; // 1 segundo

        public ResponseTimeMiddleware(RequestDelegate next, IResponseTimeLogRepository repository)
        {
            _next = next;
            _repository = repository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            var log = new ResponseTimeLog
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.Value,
                DurationMs = sw.ElapsedMilliseconds,
                StatusCode = context.Response.StatusCode,
                ClientIp = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"],
                IsSlowRequest = sw.ElapsedMilliseconds >= SlowRequestThresholdMs
            };

            await _repository.AddAsync(log);
        }
    }
}
