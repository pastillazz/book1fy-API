
using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppWriteDbContext(DbContextOptions options):DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IDomainEvent>();
        modelBuilder
            .ApplyConfigurationsFromAssembly(
                typeof(AppWriteDbContext).Assembly,
                WriteConfigurationFilter);
    }
    
    private static bool WriteConfigurationFilter(Type type)
    {
        return type.Namespace?.Contains("Configurations.Write") ?? false;
    }
    
}

