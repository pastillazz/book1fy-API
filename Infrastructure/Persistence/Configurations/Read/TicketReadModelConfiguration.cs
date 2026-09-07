using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Read;

public class TicketReadModelConfiguration:IEntityTypeConfiguration<TicketReadModel>
{
    public void Configure(EntityTypeBuilder<TicketReadModel> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.ServiceId)
            .HasColumnName("service_id");

        builder.Property(t => t.UserId)
            .HasColumnName("user_id");

        builder.Property(t => t.Status)
            .HasColumnName("status");

        builder.Property(t => t.Price)
            .HasColumnName("price");

        builder.Property(t => t.StartTimeUtc)
            .HasColumnName("start_time_utc");

        builder.Property(t => t.EndTimeUtc)
            .HasColumnName("end_time_utc");
        
        builder.HasOne(t => t.Service)
            .WithMany()
            .HasForeignKey(t => t.ServiceId);
    }
}