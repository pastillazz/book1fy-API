using Api.Abstractions;
using Api.Companies.Company;
using Api.Companies.Service;
using Api.Companies.Ticket;
using Application.Companies.Commands.AddService;
using Application.Companies.Commands.AddTicket;
using Application.Companies.Commands.CancelTicket;
using Application.Companies.Commands.CreateCompany;
using Application.Companies.Queries;
using Application.Companies.Queries.GetCompanyByEmail;
using Application.Companies.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


public class CompanyController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompany(CompanyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCompanyCommand(
                request.Name, request.Description,
                request.Email);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);

        return CreatedAtAction(
            actionName:nameof(GetCompanyById),
            routeValues:new {id=result.Value},
            value: new {Id = result.Value}
            );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);
        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyByEmail(string email, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByEmailQuery(email);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);
        return Ok(result.Value);
    }

    [HttpPost("{companyId:guid}/services")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddService(Guid companyId,
        CreateServiceRequest request, CancellationToken cancellationToken)
    {
        var command = new AddServiceCommand(
             companyId, request.Name, request.Description,
            request.OpeningTime, request.ClosingTime,
            request.WorkDays, request.Price);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);

        return CreatedAtAction(
            actionName: nameof(GetCompanyById),
            routeValues: new { id = companyId },
            value: new { Id = result.Value });
    }

    [HttpPost("{companyId:guid}/services/{serviceId:guid}/tickets")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTicket(Guid companyId, Guid serviceId,
        CreateTicketRequest request, CancellationToken cancellationToken)
    {
        
        var command = new AddTicketCommand( request.UserId,
            companyId, serviceId,
            request.StartTimeUtc, request.EndTimeUtc);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);

        return CreatedAtAction(
            actionName: nameof(GetCompanyById),
            routeValues: new { id = companyId },
            value: new { Id = result.Value });
    }

    [HttpDelete("{companyId:guid}/services/{serviceId:guid}/tickets/{ticketId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTicket(Guid companyId, Guid serviceId,
        Guid ticketId, CancellationToken cancellationToken)
    {
        var command = new CancelTicketCommand(companyId, serviceId, ticketId);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);

        return NoContent();
    }
}
