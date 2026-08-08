using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Email;
using Application.Companies.Queries;
using Application.Companies.Queries.Interfaces;
using Application.Users.Queries;
using Domain.Abstractions;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Db Configuration
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseNpgsql(configuration
                    .GetConnectionString("DefaultConnection")));
        
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