using Application.Companies.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class CompanyQueries(AppDbContext context):ICompanyQueries
{
    public async Task<CompanyResponse?> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken)
    {
       return await context.Companies
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CompanyResponse
                (c.Id, c.Name, c.Description, c.Status.ToString(), 
                    c.Email.Value,
                    c.CreatedAt,
                    c.Services.Select(s => 
                        new ServiceResponse(s.Id, s.Name,
                            s.Description, s.OpeningTime, 
                            s.ClosingTime, s.WorkDays.ToList(), 
                            s.Price)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CompanyResponse?> GetCompanyByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Companies
            .AsNoTracking()
            .Where(c => c.Email.Value == email)
            .Select(c => new CompanyResponse
                (c.Id, c.Name, c.Description, c.Status.ToString(),
                    c.Email.Value,
                    c.CreatedAt,
                    c.Services.Select(s =>
                        new ServiceResponse(s.Id, s.Name,
                            s.Description, s.OpeningTime,
                            s.ClosingTime, s.WorkDays.ToList(),
                            s.Price)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ServiceResponse?> GetServiceByIdAsync(
        Guid companyId, Guid serviceId, CancellationToken cancellationToken)
    {
        return await context.Companies
            .AsNoTracking()
            .Where(c => c.Id == companyId)
            .SelectMany(c => c.Services)
            .Where(s => s.Id == serviceId)
            .Select(s => new ServiceResponse(
                s.Id,
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
        return await context.Companies
            .AsNoTracking()
            .Where(c => c.Id == companyId)
            .SelectMany(c => c.Services)
            .Where(s => s.Id == serviceId)
            .SelectMany(s => s.Tickets)
            .Where(t => t.Id == ticketId)
            .Select(t => new TicketResponse(
                t.Id,
                t.ServiceId,
                t.UserId,
                t.Status.ToString(),
                t.StartTimeUtc,
                t.EndTimeUtc,
                t.Price))
            .FirstOrDefaultAsync(cancellationToken);
    }
}