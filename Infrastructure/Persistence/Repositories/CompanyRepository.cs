using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CompanyRepository(AppDbContext context):ICompanyRepository
{
    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Companies
            .Where(c => c.Id == id)
            .Include(c=>c.Services)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
    {
        await context.Companies.AddAsync(company, cancellationToken);
    }
    
    public void Remove(Company company)
    {
        context.Companies.Remove(company);
    }

    public async Task<bool> HasTicketExists(Guid serviceId, DateTime startTimeUtc,
        DateTime endTimeUtc, CancellationToken cancellationToken = default)
    {
        return await context.Set<Ticket>()
            .AnyAsync(t=>
                t.ServiceId == serviceId &&
                t.StartTimeUtc<endTimeUtc && t.EndTimeUtc>startTimeUtc,
                cancellationToken);
    }
}