namespace Infrastructure.Persistence.Models;

public class TicketReadModel
{
    public Guid Id{get; set;}= Guid.Empty;
    public Guid ServiceId{get; set;}= Guid.Empty;
    public Guid UserId{get; set;} = Guid.Empty;
    public string Status{get; set;}= null!;
    public DateTime StartTimeUtc{get; set;}
    public DateTime EndTimeUtc{get; set;}
    public decimal Price{get; set;}
    public ServiceReadModel Service{get; set;}= null!;
}