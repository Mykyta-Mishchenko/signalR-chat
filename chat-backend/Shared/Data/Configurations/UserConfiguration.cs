using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chat_backend.Shared.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId).ValueGeneratedOnAdd();

            builder.Property(u => u.UserName)
                .HasMaxLength(50).IsRequired();

            builder.Property(u=>u.Email)
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR(100)")
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password)
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR(100)")
                .IsRequired();

            builder.ToTable("Users");
        }
    }
}
