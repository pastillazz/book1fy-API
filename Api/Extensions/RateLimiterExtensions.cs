using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Api.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            /* options.AddFixedWindowLimiter("fixed",
                 opt =>
             {
                 opt.PermitLimit =3;
                 opt.Window = TimeSpan.FromSeconds(10);
                 opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                 opt.QueueLimit = 2;
             });

             options.AddSlidingWindowLimiter("sliding",
                 opt=>
             {
                 opt.PermitLimit = 15;
                 opt.Window = TimeSpan.FromSeconds(15);
                 opt.SegmentsPerWindow = 3;
             });

             options.AddTokenBucketLimiter("token", opt =>
             {
                 opt.TokenLimit = 100;
                 opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                 opt.TokensPerPeriod = 10;
             });

             options.AddConcurrencyLimiter("concurrency", opt =>
             {
                 opt.PermitLimit = 10;
             });*/

            options.AddPolicy("sliding", context =>
            {
                var ipAddress = context.Connection
                                    .RemoteIpAddress?.ToString()
                                ?? "unknown";
                
                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 3
                    });
            });

            options.AddPolicy("token", context =>
            {
                var userId = context.User.
                    FindFirstValue(ClaimTypes.NameIdentifier) 
                             ?? context.Connection.RemoteIpAddress?.ToString() 
                             ?? "unknown";
                
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: userId,
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 100,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        TokensPerPeriod = 10
                    });
            });
        });
        return services;
    }
}