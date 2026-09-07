using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Read;

public class TicketReadModelConfiguration:IEntityTypeConfiguration<TicketReadModel>
{
    public void Configure(EntityTypeBuilder<TicketReadModel> builder)
    {
        builder.ToTable("Tickets");
        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.Service)
            .WithMany()
            .HasForeignKey(t => t.ServiceId);
    }
}