using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TicketConfiguration:IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();
        
        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(s=>s.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(s => s.StartTimeUtc)
            .HasColumnName("start_time_utc")
            .IsRequired();
        
        builder.Property(s => s.EndTimeUtc)
            .HasColumnName("end_time_utc")
            .IsRequired();
    }
}