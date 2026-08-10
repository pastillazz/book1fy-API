using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Email;
using Application.Companies.Queries;
using Application.Companies.Queries.Interfaces;
using Application.Users.Queries;
using Domain.Abstractions;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Email;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {   
        //Outbox Configuration
        services
            .AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddQuartz(configure =>
            {
                var jobKey=new JobKey(nameof(ProcessOutboxMessagesJob));
                
                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule => 
                                        schedule.WithIntervalInSeconds(10)
                                            .RepeatForever())); });
        
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        //Db Configuration
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {   
            var interceptor = serviceProvider
                .GetRequiredService
                    <ConvertDomainEventsToOutboxMessagesInterceptor>();
            options
                .UseNpgsql(configuration
                    .GetConnectionString("DefaultConnection"))
                .AddInterceptors(interceptor);
        });
        
        //Jwt Configuration
        services.Configure<JwtSettings>
            (configuration.GetSection(JwtSettings.SectionName));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        //DI Configuration
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPasswordHasher,PasswordHasher>();
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<ICompanyQueries, CompanyQueries>();
        
        //Smtp Configuration
        services.Configure<SmtpSettings>(
            configuration.GetSection(SmtpSettings.SectionName));
        
        services.AddTransient<IEmailService, SmtpEmailService>();
        
        return services;
    }
}