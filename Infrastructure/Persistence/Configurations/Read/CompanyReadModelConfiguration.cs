using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Read;

public class CompanyReadModelConfiguration:IEntityTypeConfiguration<CompanyReadModel>
{
    public void Configure(EntityTypeBuilder<CompanyReadModel> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .HasColumnName("name");

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.Status)
            .HasColumnName("status");

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(c => c.OwnerId)
            .HasColumnName("owner_id");

        builder.Property(c => c.Email)
            .HasColumnName("email");
        builder.HasMany(c => c.Services)
            .WithOne()
            .HasForeignKey(s => s.CompanyId);
    }
}
