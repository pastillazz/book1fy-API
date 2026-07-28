using Domain.Enums;

namespace Application.Companies.Queries;

public record CompanyResponse(
    Guid Id,
    string Name,
    string Description,
    string Status,
    string Email,
    DateTime CreatedAt,
    IReadOnlyCollection<ServiceResponse> Services);