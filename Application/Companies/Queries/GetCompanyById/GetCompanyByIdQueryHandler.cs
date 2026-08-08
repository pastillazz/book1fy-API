using Application.Common.Abstractions.Interfaces;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Errors;
using Domain.Shared;

namespace Application.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler(ICompanyQueries companyQueries) : 
    IQueryHandler<GetCompanyByIdQuery,CompanyResponse>
{
    public async Task<Result<CompanyResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company= await companyQueries.GetCompanyByIdAsync(request.Id,cancellationToken);

        if (company is null) return CompanyErrors.CompanyNotFound;

        return company;
    }
}
