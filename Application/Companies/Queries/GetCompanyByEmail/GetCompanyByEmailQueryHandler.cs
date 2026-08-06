using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Abstractions;
using Domain.Errors;

namespace Application.Companies.Queries.GetCompanyByEmail;

public class GetCompanyByEmailQueryHandler(ICompanyQueries companyQueries) : 
    IQueryHandler<GetCompanyByEmailQuery,CompanyResponse>
{
    public async Task<Result<CompanyResponse>> Handle(GetCompanyByEmailQuery request, CancellationToken cancellationToken)
    {
        var company = await companyQueries.GetCompanyByEmailAsync(request.Email, cancellationToken);
        if (company is null) return CompanyErrors.CompanyNotFound;

        return company;
    }
}
