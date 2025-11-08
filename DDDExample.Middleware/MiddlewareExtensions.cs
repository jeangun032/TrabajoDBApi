using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDDExample.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddResponseTimeMiddleware(this IServiceCollection services)
        {
            services.Configure<ResponseTimeOptions>(options =>
            {
                options.ThresholdMs = 500;
                options.LogSlowRequests = true;
            });

            return services.AddScoped<ResponseTimeMiddleware>();
        }

        public static IServiceCollection AddResponseTimeMiddleware(this IServiceCollection services,
            Action<ResponseTimeOptions> configureOptions)
        {
            services.Configure(configureOptions);
            return services.AddResponseTimeMiddleware();
        }

        public static IApplicationBuilder UseResponseTimeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ResponseTimeMiddleware>();
        }
    }

    public class ResponseTimeOptions
    {
        public long ThresholdMs { get; set; } = 500;
        public bool LogSlowRequests { get; set; } = true;
    }
}
