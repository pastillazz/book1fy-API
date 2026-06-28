using Domain.Abstractions;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Service : AggregateRoot
{   private readonly List<DayOfWeek> _workDays=new();
    private readonly List<Ticket> _tickets=new();
    private Service(Guid id,
        string name, string description, TimeSpan openingTime, TimeSpan closingTime,
        List<DayOfWeek> workDays) : base(id)
    {
        Name = name;
        Description = description;
        OpeningTime = openingTime;
        ClosingTime = closingTime;
        _workDays = workDays;
    }

    protected Service()
    {
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public TimeSpan OpeningTime { get; private set; }
    public TimeSpan ClosingTime { get; private set; }
    public IReadOnlyCollection<DayOfWeek> WorkDays => _workDays;
    public IReadOnlyCollection<Ticket> Tickets => _tickets;
}
    