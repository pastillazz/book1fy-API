using Application.Common.Abstractions.Events;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options, 
    IPublisher publisher):DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IDomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
}

