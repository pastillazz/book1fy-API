namespace Application.Companies.Queries;

public record ServiceResponse(
    Guid Id, 
    string Name,
    string Description,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime,
    IReadOnlyList<DayOfWeek> WorkDays,
    decimal Price);
