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
}