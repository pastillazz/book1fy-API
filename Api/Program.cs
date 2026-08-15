using System.Text.Json.Serialization;
using Api.Authentication;
using Api.Endpoints;
using Api.Extensions;
using Api.Middleware;
using Application;
using Application.Common.Abstractions.Authentication;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter()));

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(
    options =>{options.SerializerOptions.Converters
        .Add(new JsonStringEnumConverter());} );

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCustomRateLimiter();
var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();
app.MapCompanyEndpoints();



app.Run();


