using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number")
            .IsRequired()
            .HasMaxLength(20);
        builder.ComplexProperty(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);
        });
        builder.ComplexProperty(u => u.Password, passwordBuilder =>
        {
            passwordBuilder.Property(p=> p.Hash)
                .HasColumnName("password_hash")
                .IsRequired()
                .HasMaxLength(255);
        });
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