namespace Application.Companies.Queries.Responses;

public record ServiceResponse(
    Guid Id, 
    Guid CompanyId,
    string Name,
    string? Description,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime,
    IReadOnlyList<DayOfWeek> WorkDays,
    decimal Price);
