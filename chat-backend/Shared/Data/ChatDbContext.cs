using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace chat_backend.Shared.Data
{
    public class ChatDbContext: DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UsersRoles { get; set; }
        public DbSet<UserProfile> UsersProfile { get; set; }
        public DbSet<RefreshSession> RefreshSessions { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
