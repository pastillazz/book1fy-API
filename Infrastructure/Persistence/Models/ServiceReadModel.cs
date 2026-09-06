namespace Infrastructure.Persistence.Models;

public class ServiceReadModel
{
    public Guid Id {get; set;}
    public Guid CompanyId { get;  set; }
    public string Name {get; set;}
    public string Description {get; set;} 
    public TimeSpan OpeningTime {get; set;}
    public TimeSpan ClosingTime {get; set;}
    public IReadOnlyList<DayOfWeek> WorkDays {get; set;}
    public decimal Price {get; set;}
}