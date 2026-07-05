using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CompanyConfiguration:IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(c => c.Id);
        
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
        
        builder.HasMany(c=>c.Services)
            .WithOne()
            .HasForeignKey(s=>s.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}