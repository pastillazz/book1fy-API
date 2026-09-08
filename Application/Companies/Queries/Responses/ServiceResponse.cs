using Application.Common.Abstractions.Link;

namespace Application.Companies.Queries.Responses;

public record ServiceResponse(
    Guid Id,
    Guid CompanyId,
    string Name,
    string? Description,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime,
    IReadOnlyList<DayOfWeek> WorkDays,
    decimal Price)
{
    public List<Link> Links { get; set; } = new();
};
