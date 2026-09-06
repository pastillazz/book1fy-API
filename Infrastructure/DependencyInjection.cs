using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Email;
using Application.Companies.Queries.Interfaces;
using Application.Users.Queries;
using Domain.Abstractions;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Email;
using Infrastructure.Health;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Outbox;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection
        services, IConfiguration configuration)
    {   
        //Outbox Configuration
        services
            .AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        services.AddOptions<OutboxSettings>()
            .Bind(configuration.GetSection(OutboxSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var outboxSettings = configuration
            .GetSection(OutboxSettings.SectionName)
            .Get<OutboxSettings>()!;
        
        services.AddQuartz(configure =>
            {
                var jobKey=new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule
                                            .WithIntervalInSeconds
                                                (outboxSettings
                                                    .IntervalInSeconds)
                                            .RepeatForever())); });
        
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        //Db Configuration
        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

       
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. " +
                "Set it with: dotnet user-secrets set " +
                "\"ConnectionStrings:DefaultConnection\" \"<value>\"");
        }
        
        //Write DbContext Configuration
        services.AddDbContext<AppWriteDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider
                .GetRequiredService
                    <ConvertDomainEventsToOutboxMessagesInterceptor>();
            options
                .UseNpgsql(connectionString)
                .AddInterceptors(interceptor);
        });
        
        //Read DbContext Configuration
        services.AddDbContext<AppReadDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        //Jwt Configuration
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        //DI Configuration
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPasswordHasher,PasswordHasher>();
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<ICompanyQueries, CompanyQueries>();
        
        //Smtp Configuration
        services.AddOptions<SmtpSettings>()
            .Bind(configuration.GetSection(SmtpSettings.SectionName))
            .ValidateDataAnnotations()
            .Validate(settings => 
            {
                
                if (settings.Host.Contains("gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    if (settings.Port != 587 && settings.Port != 465)
                    {
                        return false;
                    }
                }
                return true;
            })
            .ValidateOnStart();
        
        services.AddTransient<IEmailService, SmtpEmailService>();
        
        //HealthChecks Configuration
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>
                ("PostgreSQL Custom Database Health Check",
                    HealthStatus.Unhealthy)
            .AddNpgSql(connectionString)
            .AddDbContextCheck<AppWriteDbContext>();
        return services;
    }
}