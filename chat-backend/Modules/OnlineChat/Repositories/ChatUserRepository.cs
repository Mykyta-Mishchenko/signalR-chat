using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.OnlineChat.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly ChatDbContext _dbContext;
        public ChatUserRepository(ChatDbContext dbContext) { 
            _dbContext = dbContext;
        }
        public async Task<List<User>> GetUsersByNameOrEmail(string request)
        {
            return await _dbContext.Users
                .Where(u => u.UserName.Contains(request) || u.Email.Contains(request))
                .ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
