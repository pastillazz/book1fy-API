using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : 
    IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(x => x.Id);
        
        builder.Property(c => c.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.Type)
            .IsRequired();
        
        builder.Property(x => x.Content)
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(x => x.OccurredOnUtc)
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc)
            .IsRequired(false);

        builder.Property(x => x.Error)
            .IsRequired(false);

        builder.Property(x => x.RetryCount)
            .HasDefaultValue(0)
            .IsRequired();
        
        builder.HasIndex(x => new { x.ProcessedOnUtc, x.OccurredOnUtc });
    }
}