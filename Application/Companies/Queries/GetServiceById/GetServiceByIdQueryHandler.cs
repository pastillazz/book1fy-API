using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Errors;
using Domain.Shared;

namespace Application.Companies.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(ICompanyQueries companyQueries)
    : IQueryHandler<GetServiceByIdQuery, ServiceResponse>
{
    public async Task<Result<ServiceResponse>> Handle(
        GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await companyQueries.GetServiceByIdAsync(
            request.CompanyId, request.ServiceId, cancellationToken);

        if (service is null) return ServiceErrors.NotFound;

        return service;
    }
}
