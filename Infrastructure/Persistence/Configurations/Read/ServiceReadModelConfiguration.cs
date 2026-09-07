using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Read;

public class ServiceReadModelConfiguration:IEntityTypeConfiguration<ServiceReadModel>
{
    public void Configure(EntityTypeBuilder<ServiceReadModel> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(s => s.Id);
        
    }
}