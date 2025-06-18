using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.OnlineChat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatDbContext _dbContext;
        public MessageRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Message?> CreateMessageAsync(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetChatMessagesAsync(int chatId)
        {
            return await _dbContext.Messages.Where(m => m.ChatId == chatId)
                .OrderBy(m=>m.TimeStamp)
                .ToListAsync();
        }

        public async Task<Message?> GetLastChatMessageAsync(int chatId)
        {
            return await _dbContext.Messages
                .Where(m=>m.Chat.Id == chatId)
                .OrderByDescending(m => m.TimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetUnreadedChatMessagesAsync(int chatId)
        {
            return await _dbContext.Messages
                .Where(m => m.ChatId == chatId && m.IsRead == false)
                .ToListAsync();
        }

        public async Task<Message?> SetMessageAsReadAsync(int messageId)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if(message is null)
            {
                return null;
            }

            message.IsRead = true;
            await _dbContext.SaveChangesAsync();

            return message;
        }
    }
}
