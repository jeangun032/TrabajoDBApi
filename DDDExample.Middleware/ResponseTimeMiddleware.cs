using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DDDExample.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseTimeOptions _options;
        private readonly ILogger<ResponseTimeMiddleware> _logger;

        public ResponseTimeMiddleware(RequestDelegate next, IOptions<ResponseTimeOptions> options, ILogger<ResponseTimeMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();
            var duration = stopwatch.ElapsedMilliseconds;

            context.Response.Headers["X-Response-Time"] = $"{duration}ms";

            if (_options.LogSlowRequests && duration > _options.ThresholdMs)
            {
                _logger.LogWarning("Request {Method} {Path} took {Duration}ms (Threshold: {Threshold}ms)",
                    context.Request.Method,
                    context.Request.Path,
                    duration,
                    _options.ThresholdMs);
            }
        }
    }
}
