using Asp.Versioning.ApiExplorer;
using Scalar.AspNetCore;

namespace Api.Extensions;

public static class OpenApiExtensions
{
    public static WebApplication UseConfiguredOpenApi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        
        var descriptions = app
            .DescribeApiVersions();
        
        app.MapOpenApi()
            .WithDocumentPerVersion();
        
        app.MapScalarApiReference(options =>
        {
            foreach (var description in descriptions)
            {
                options
                    .AddDocument(description.GroupName, 
                        $"API {description.GroupName.ToUpperInvariant()}",
                        $"/openapi/{description.GroupName}.json");
                
            } 
        });
        
        app.UseSwaggerUI(options =>
        {
            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint(
                    $"/openapi/{description.GroupName}.json",
                    $"API {description.GroupName.ToUpperInvariant()}"
                );
            }
            options.RoutePrefix = "swagger"; 
            
        });

        return app;
    }
}