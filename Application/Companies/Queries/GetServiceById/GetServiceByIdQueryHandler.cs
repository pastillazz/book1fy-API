using Application.Common.Abstractions.Interfaces;
using Application.Common.Abstractions.Link;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Errors;
using Domain.Shared;

namespace Application.Companies.Queries.GetServiceById;

public class GetServiceByIdQueryHandler
    : IQueryHandler<GetServiceByIdQuery, ServiceResponse>
{
    private readonly ICompanyQueries _companyQueries;
    private readonly ILinkService _linkService;
    public GetServiceByIdQueryHandler(ICompanyQueries companyQueries,
        ILinkService linkService)
    {
        _companyQueries = companyQueries;
        _linkService = linkService;
    }
    public async Task<Result<ServiceResponse>> Handle(
        GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await _companyQueries.GetServiceByIdAsync(
            request.CompanyId, request.ServiceId, cancellationToken);

        if (service is null) return ServiceErrors.NotFound;
        
        AddLinksForProducts(service);
        
        return service;
    }

    private void AddLinksForProducts(ServiceResponse serviceResponse)
    {
        serviceResponse.Links.Add(_linkService
            .Generate("GetServiceById", 
                new {
                    CompanyId= serviceResponse.CompanyId,
                   ServiceId= serviceResponse.Id }, "self", "GET"));
        
        serviceResponse.Links.Add(_linkService
            .Generate("AddService", 
                new {
                    CompanyId= serviceResponse.CompanyId},
                "create-service", "POST"));
        
    }
}
