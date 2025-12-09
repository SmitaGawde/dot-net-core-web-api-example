using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Throttlingapi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, RateLimiter> _limiters = new();


        public ThrottlingMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            string clientid = GetClientId(httpContext);

            var limiter = _limiters.GetOrAdd(clientid, async =>
            {
                return new TokenBucketRateLimiter(
                    new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 5,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                        TokensPerPeriod = 1,
                        AutoReplenishment = true
                    }
                    );
                

            });
            using var lease = await limiter.AcquireAsync(1, httpContext.RequestAborted);
            //using var lease = rateLimitLease;
            if (!lease.IsAcquired)
            {
                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await httpContext.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }

            await _next(httpContext);
        }

        private string GetClientId(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ThrottlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseThrottlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThrottlingMiddleware>();
        }
    }
}
