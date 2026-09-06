using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppReadDbContext(DbContextOptions options):DbContext(options)
{
    public DbSet<UserReadModel> Users => Set<UserReadModel>();
    public DbSet<CompanyReadModel> Companies => Set<CompanyReadModel>();
    public DbSet<ServiceReadModel> Services => Set<ServiceReadModel>();
    public DbSet<TicketReadModel> Tickets => Set<TicketReadModel>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .ApplyConfigurationsFromAssembly(
                typeof(AppReadDbContext).Assembly,
                ReadConfigurationFilter);
    }
    
    private static bool ReadConfigurationFilter(Type type)
    {
        return type.Namespace?.Contains("Configurations.Read") ?? false;
    }
    
}

