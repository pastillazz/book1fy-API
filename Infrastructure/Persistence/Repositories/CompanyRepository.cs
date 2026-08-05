using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CompanyRepository(AppDbContext context):ICompanyRepository
{
    
    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Companies
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }


    public void Add(Company company)=> context.Companies.Add(company);
    
    
    public void Remove(Company company)=> context.Companies.Remove(company);
    

    public async Task<Company?> GetCompleteByIdAsync(Guid companyId, 
        Guid serviceId, CancellationToken cancellationToken = default)
    {
       return await context.Companies
            .Include(c =>
                c.Services.Where(s => s.Id == serviceId))
            .ThenInclude(s => s.Tickets)
            .FirstOrDefaultAsync(c => c.Id == companyId,cancellationToken);
    }
}