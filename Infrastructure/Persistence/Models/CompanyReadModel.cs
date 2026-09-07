namespace Infrastructure.Persistence.Models;

public class CompanyReadModel
{   
    public Guid Id { get;  set; }= Guid.Empty;
    public string Name { get;  set; }= null!;
    public string Description { get; set; }= null!;
    public string Email { get;  set; }= null!;
    public Guid OwnerId { get;  set; }= Guid.Empty;
    public string Status { get;  set; }= null!;
    public DateTime CreatedAt { get;  set; }
    public List<ServiceReadModel> Services { get;  set; }= null!;
}