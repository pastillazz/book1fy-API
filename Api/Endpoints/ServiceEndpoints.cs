using Api.Extensions;
using Application.Companies.Queries.GetServices;
using Application.Companies.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("services")
            .WithTags("Services")
            .RequireAuthorization()
            .RequireRateLimiting("token");

        group.MapGet("", GetServices)
            .WithName(nameof(GetServices))
            .RequireAuthorization("UserPolicy");
    }
    private static async Task<Results<Ok<PagedList<ServiceResponse>>,
            ProblemHttpResult>> GetServices(string? searchTerm, string?
                sortColumn, string? sortOrder, int page, int pageSize,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetServicesQuery(searchTerm, sortColumn, sortOrder,
            page, pageSize);
        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return result.HandleFailure();
        }

        return TypedResults.Ok(result.Value);
    }
}
