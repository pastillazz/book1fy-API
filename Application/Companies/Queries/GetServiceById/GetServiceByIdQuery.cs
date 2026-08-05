using Application.Abstractions.Interfaces;

namespace Application.Companies.Queries.GetServiceById;

public record GetServiceByIdQuery(
    Guid CompanyId,
    Guid ServiceId)
    : IQuery<ServiceResponse>;
