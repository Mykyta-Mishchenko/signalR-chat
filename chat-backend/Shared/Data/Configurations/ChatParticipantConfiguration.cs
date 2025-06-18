using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chat_backend.Shared.Data.Configurations
{
    public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
    {
        public void Configure(EntityTypeBuilder<ChatParticipant> builder)
        {
            builder.HasKey(c=> c.Id);

            builder.Property(c=> c.Id).ValueGeneratedOnAdd();

            builder.Property(c=> c.ChatId).IsRequired();

            builder.Property(c=> c.UserId).IsRequired();

            builder.HasOne(c=> c.Chat)
                .WithMany(u => u.Participants)
                .HasForeignKey(c=> c.ChatId);

            builder.HasOne(c=> c.User)
                .WithMany(u => u.ChatParticipants)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ChatParticipants");
        }
    }
}
