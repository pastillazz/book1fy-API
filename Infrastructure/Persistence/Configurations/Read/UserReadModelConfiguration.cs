using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Read;

public class UserReadModelConfiguration: IEntityTypeConfiguration<UserReadModel>
{
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.FirstName)
            .HasColumnName("first_name");

        builder.Property(u => u.LastName)
            .HasColumnName("last_name");

        builder.Property(u => u.Username)
            .HasColumnName("username");

        builder.Property(u => u.Email)
            .HasColumnName("email");

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number");
    }
}