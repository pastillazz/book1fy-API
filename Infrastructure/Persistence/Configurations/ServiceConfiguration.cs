using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class ServiceConfiguration:IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.CompanyId)
            .HasColumnName("company_id")
            .IsRequired();
        
        builder.Property(s => s.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.OpeningTime)
            .HasColumnName("opening_time")
            .IsRequired();

        builder.Property(s => s.ClosingTime)
            .HasColumnName("closing_time")
            .IsRequired();
        
        builder.PrimitiveCollection(s => s.WorkDays)
            .HasColumnName("work_days")
            .ElementType()
            .HasConversion<string>();
       
        builder.HasMany(s => s.Tickets)
            .WithOne()
            .HasForeignKey(t => t.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}