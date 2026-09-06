using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class CompanyQueries(AppReadDbContext context):ICompanyQueries
{
    public async Task<CompanyResponse?> GetCompanyByIdAsync(Guid id, 
        CancellationToken cancellationToken)
    {
       return await context.Companies
            .Where(c => c.Id == id)
            .Select(c => new CompanyResponse
                (c.Id, 
                    c.Name, 
                    c.Description,
                    c.Status, 
                    c.Email,
                    c.CreatedAt,
                    c.Services.Select(s=> new ServiceResponse(
                            s.Id,
                        s.CompanyId,
                        s.Name,
                        s.Description,
                        s.OpeningTime,
                        s.ClosingTime,
                        s.WorkDays.ToList(),
                        s.Price))
                        .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<ServiceResponse?> GetServiceByIdAsync(
        Guid companyId, Guid serviceId, CancellationToken cancellationToken)
    {
        return await context.Services
            .Where(s => s.Id == serviceId && 
                        s.CompanyId == companyId)
            .Select(s => new ServiceResponse(
                s.Id,
                s.CompanyId,
                s.Name,
                s.Description,
                s.OpeningTime,
                s.ClosingTime,
                s.WorkDays.ToList(),
                s.Price))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TicketResponse?> GetTicketByIdAsync(
        Guid companyId, Guid serviceId, Guid ticketId,
        CancellationToken cancellationToken)
    {
        return await context.Tickets
            .Where(t => t.Id == ticketId 
                        && t.ServiceId == serviceId &&
                        t.Service.CompanyId == companyId)
            .Select(t => new TicketResponse(
                t.Id,
                t.ServiceId,
                t.UserId,
                t.Status,
                t.StartTimeUtc,
                t.EndTimeUtc,
                t.Price))
            .FirstOrDefaultAsync(cancellationToken);
    }
}