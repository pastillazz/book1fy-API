using System.Text.Json.Serialization;
using Api.Authentication;
using Api.Endpoints;
using Api.Extensions;
using Api.Middleware;
using Api.Seeding;
using Api.Services;
using Application;
using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Link;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter()));

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddAuthorizationPolicies()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(
    options =>{options.SerializerOptions.Converters
        .Add(new JsonStringEnumConverter());} );

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<ILinkService, LinkService>();

//Admin Seeding Configuration
builder.Services.AddOptions<AdminSeedSettings>()
    .Bind(builder.Configuration.GetSection(AdminSeedSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHostedService<AdminSeederHostedService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCustomRateLimiter();

//Api-Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat="'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }).AddOpenApi();



var app = builder.Build();

app.UseExceptionHandler();

app.UseConfiguredOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

var versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionedGroup.MapCompanyEndpoints();
versionedGroup.MapServiceEndpoints();

app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous().DisableRateLimiting();

app.Run();


