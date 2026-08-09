using Application.Common.Abstractions.Interfaces;

namespace Application.Companies.Commands.AddService;

public record AddServiceCommand(
    Guid CompanyId,
    string Name,
    string Description,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime,
    List<DayOfWeek> WorkDays,
    decimal Price):ICommand<Guid>;