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
        
        builder.Property(s => s.CompanyId)
            .HasColumnName("company_id");
        
        builder.Property(s => s.Name)
            .HasColumnName("name");
        
        builder.Property(s => s.Description)
            .HasColumnName("description");
        
        builder.Property(s => s.OpeningTime)
            .HasColumnName("opening_time");
        
        builder.Property(s => s.ClosingTime)
            .HasColumnName("closing_time");
        
        builder.Property(s => s.Price)
            .HasColumnName("price");
        
        builder.PrimitiveCollection(s => s.WorkDays)
            .HasColumnName("work_days")
            .ElementType()
            .HasConversion<string>();
    }
}