namespace Api.Companies.Service;

public record CreateServiceRequest(
    string Name,
    string Description,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime,
    List<DayOfWeek> WorkDays,
    decimal Price);
