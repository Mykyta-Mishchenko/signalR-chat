using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.OnlineChat.Repositories
{
    public class OnlineUsersRepository : IOnlineUsersRepository
    {
        public ChatDbContext _dbContext { get; set; }
        public OnlineUsersRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUserOnlineAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return user?.IsOnline ?? false;
        }

        public async Task SetUserOfflineAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user is null) return;

            user.IsOnline = false;
            user.LastSeenAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetUserOnlineAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user is null) return;

            user.IsOnline = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
