using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chat_backend.Shared.Data.Configurations
{
    public class SessionsConfiguration : IEntityTypeConfiguration<RefreshSession>
    {
        public void Configure(EntityTypeBuilder<RefreshSession> builder)
        {
            builder.HasKey(s => s.SessionId);

            builder.Property(r => r.UserId).ValueGeneratedOnAdd();

            builder.Property(s => s.UserId).IsRequired();

            builder.Property(s => s.RefreshToken).IsRequired();

            builder.HasIndex(s => s.RefreshToken).IsUnique();

            builder.Property(s => s.ExpireTime).IsRequired();

            builder.HasOne(s => s.User)
                .WithMany(s => s.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RefreshSessions");
        }
    }
}
