using Api.Companies.Company;
using Api.Companies.Service;
using Api.Companies.Ticket;
using Api.Extensions;
using Application.Companies.Commands.AddService;
using Application.Companies.Commands.AddTicket;
using Application.Companies.Commands.CancelTicket;
using Application.Companies.Commands.CreateCompany;
using Application.Companies.Queries.GetCompanyById;
using Application.Companies.Queries.GetServiceById;
using Application.Companies.Queries.GetTicketById;
using Application.Companies.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

public static class CompanyEndpoints
{
    
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("companies")
            .WithTags("Companies")
            .RequireAuthorization()
            .RequireRateLimiting("token");
        
        
        group.MapPost("",CreateCompany);
        
        
        group.MapGet("{id:guid}", GetCompanyById)
            .WithName(nameof(GetCompanyById))
            .RequireAuthorization("AdminPolicy");

        group.MapPost("{companyId:guid}/services", AddService)
            .WithName(nameof(AddService));
        
        group.MapGet("{companyId:guid}/services/{serviceId:guid}", 
                GetServiceById)
            .WithName(nameof(GetServiceById))
            .RequireAuthorization("UserPolicy");

        group.MapPost("{companyId:guid}/services" +
                      "/{serviceId:guid}/tickets",
            AddTicket);
        
        group.MapGet("{companyId:guid}/services" +
                     "/{serviceId:guid}/tickets/{ticketId:guid}",
                GetTicketById)
            .WithName(nameof(GetTicketById));
        
        group.MapDelete("{companyId:guid}/services" +
                        "/{serviceId:guid}/tickets/{ticketId:guid}",
            CancelTicket);
            
    }
    
    private static async Task<Results<CreatedAtRoute<Guid>,
            ProblemHttpResult>> CreateCompany
        (CompanyRequest request, ISender sender,
            CancellationToken cancellationToken)
    {
        var command = new CreateCompanyCommand(
            request.Name, request.Description,
            request.Email);

        var result = await sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.HandleFailure();
        }
        
        return TypedResults.CreatedAtRoute(
            value: result.Value,
            routeName:nameof(GetCompanyById),
            routeValues: new { id = result.Value });
    }
    
    private static async Task<Results<Ok<CompanyResponse>,
        ProblemHttpResult>> GetCompanyById(
        Guid id,ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            return result.HandleFailure();
        }
        return TypedResults.Ok(result.Value);
    }
    
    private static async Task<Results<CreatedAtRoute<Guid>,
        ProblemHttpResult>> AddService(Guid companyId,
        CreateServiceRequest request,
        ISender sender, CancellationToken cancellationToken)
    {
        var command = new AddServiceCommand(
            companyId, request.Name, request.Description,
            request.OpeningTime, request.ClosingTime,
            request.WorkDays, request.Price);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.HandleFailure();
        }
        
        return TypedResults.CreatedAtRoute(
            value: result.Value,
            routeName: nameof(GetServiceById),
            routeValues: new { companyId, serviceId = result.Value });
    }
    
    private static async Task<Results<Ok<ServiceResponse>,
        ProblemHttpResult>> GetServiceById(
        Guid companyId, Guid serviceId, 
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetServiceByIdQuery(companyId, serviceId);
        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure) return result.HandleFailure();

        return TypedResults.Ok(result.Value);
    }
    
    private static async Task<Results<CreatedAtRoute<Guid>,
        ProblemHttpResult>> AddTicket(Guid companyId, Guid serviceId,
        CreateTicketRequest request, ISender sender,
        CancellationToken cancellationToken)
    {

        var command = new AddTicketCommand(
            companyId, serviceId,
            request.StartTimeUtc, request.EndTimeUtc);

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return result.HandleFailure();

        return TypedResults.CreatedAtRoute(
            value: result.Value,
            routeName: nameof(GetTicketById),
            routeValues: new { companyId, serviceId, ticketId = result.Value });
    }
    
    private static async Task<Results<Ok<TicketResponse>, ProblemHttpResult>> 
        GetTicketById(
            Guid companyId, Guid serviceId, Guid ticketId,
            ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetTicketByIdQuery(companyId, serviceId, ticketId);
        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure) return result.HandleFailure();

        return TypedResults.Ok(result.Value);
    }
    
    private static async Task<Results<NoContent, ProblemHttpResult>> 
        CancelTicket(Guid companyId, Guid serviceId,
            Guid ticketId,ISender sender, CancellationToken cancellationToken)
    {
        var command = new CancelTicketCommand(companyId, serviceId, ticketId);

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return result.HandleFailure();

        return TypedResults.NoContent();
    }
}
