using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.GetServiceById;

public record GetServiceByIdQuery(
    Guid CompanyId,
    Guid ServiceId)
    : IQuery<ServiceResponse>;
