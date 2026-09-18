using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Write;

internal sealed class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Reconstruct(value))
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");
        
        builder.Property(u=> u.Password)
          .HasConversion(
                password => password.Hash,
                value => Password.Reconstruct(value))
            .IsRequired()
            .HasColumnName("password")
            .HasMaxLength(255);

        builder.ComplexProperty(u => u.FullName, fullNameBuilder =>
        {
            fullNameBuilder.Property(f => f.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(100);

            fullNameBuilder.Property(f => f.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}
