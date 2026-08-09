using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class CompanyConfiguration:IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c=>c.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(c => c.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasColumnName("status");
        
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.HasIndex(c => c.OwnerId);
        
        builder.ComplexProperty(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(c => c.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);
        });
        
        builder.HasMany(c=>c.Services)
            .WithOne()
            .HasForeignKey(s=>s.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}