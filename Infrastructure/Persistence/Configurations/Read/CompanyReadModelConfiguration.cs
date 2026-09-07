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

        builder.HasMany(c => c.Services)
            .WithOne()
            .HasForeignKey(s => s.CompanyId);
    }
}