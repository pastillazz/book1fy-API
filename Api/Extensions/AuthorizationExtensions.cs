using Domain.Entities;

namespace Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole(Role.Admin));
            options.AddPolicy("UserPolicy", policy =>
                policy.RequireRole(Role.Admin, Role.User));
        });

        return services;
    }
}