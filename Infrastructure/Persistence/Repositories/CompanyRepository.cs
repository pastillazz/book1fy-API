using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CompanyRepository(AppWriteDbContext context):ICompanyRepository
{
    
    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification( new GetCompanyByIdSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(Company company)=> context.Companies.Add(company);
    
    
    public void Remove(Company company)=> context.Companies.Remove(company);
    

    public async Task<Company?> GetCompleteByIdAsync(Guid companyId, 
        Guid serviceId, CancellationToken cancellationToken = default)
    {
       return await ApplySpecification(
               new GetCompleteCompanyByIdSpecification(companyId, serviceId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<Company> ApplySpecification(Specification<Company> specification)
    {
        return SpecificationEvaluator.GetQuery(context.Companies,specification);
    }
}
