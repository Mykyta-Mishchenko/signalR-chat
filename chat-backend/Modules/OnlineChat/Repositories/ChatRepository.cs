using chat_backend.Modules.OnlineChat.Interfaces;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.OnlineChat.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _dbContext;
        public ChatRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatParticipant?> AddParticipantToChatAsync(int chatId, int participantId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            var participant = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == participantId);

            if(chat is null || participant is null)
            {
                return null;
            }

            var chatParticipant = await _dbContext.ChatParticipants
                .FirstOrDefaultAsync(x=>x.ChatId == chatId && x.UserId == participantId);

            if(chatParticipant != null)
            {
                return chatParticipant;
            }

            var newChatParticipant = new ChatParticipant
            {
                ChatId = chatId,
                UserId = participantId
            };

            await _dbContext.ChatParticipants.AddAsync(newChatParticipant);
            await _dbContext.SaveChangesAsync();

            return newChatParticipant;
        }

        public async Task<Chat?> CreateChatWithParticipantsAsync(string chatName, List<int> participantsIds)
        {
            var chat = new Chat
            {
                Name = chatName
            };

            await _dbContext.Chats.AddAsync(chat);
            await _dbContext.SaveChangesAsync();

            foreach(var participantId in participantsIds)
            {
                await AddParticipantToChatAsync(chat.Id, participantId);
            }

            return chat;
        }
    }
}
