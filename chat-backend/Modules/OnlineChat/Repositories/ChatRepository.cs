using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
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

        public async Task<Chat?> CreateChatWithParticipantsAsync(Chat chat, List<int> participantsIds)
        {
            await _dbContext.Chats.AddAsync(chat);
            await _dbContext.SaveChangesAsync();

            foreach(var participantId in participantsIds)
            {
                await AddParticipantToChatAsync(chat.Id, participantId);
            }

            return chat;
        }

        public async Task<Chat?> GetChatInfoWithParticipantsAsync(int chatId)
        {
            return await _dbContext.Chats.Where(c => c.Id == chatId)
                .Include(c => c.Participants)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Chat?> GetPersonalChatByParticipantsAsync(List<int> participantsIds)
        {
            var potentialChats = await _dbContext.Chats
                .Where(c => c.ChatType == ChatType.Personal && c.Participants.Count == participantsIds.Count)
                .Include(c => c.Participants)
                .ToListAsync();

            foreach (var existingChat in potentialChats)
            {
                var existingParticipantIds = existingChat.Participants.Select(p => p.Id).OrderBy(id => id);
                var newParticipantIds = participantsIds.OrderBy(id => id);

                if (existingParticipantIds.SequenceEqual(newParticipantIds))
                {
                    return existingChat;
                }
            }
            return null;
        }

        public async Task<List<Chat>> GetUserChatsAsync(int userId)
        {
            return await _dbContext.Chats.Where(c => c.Participants.Any(p => p.UserId == userId)).ToListAsync();
        }

        public async Task<List<Chat>> GetUserChatsWithParticipantsAsync(int userId)
        {
            return await _dbContext.Chats
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public async Task SetChatOwnerAsync(int chatId, int ownerId)
        {
            var chatParticipant = await _dbContext.ChatParticipants
                .FirstOrDefaultAsync(c => c.UserId == ownerId && c.ChatId == chatId);

            if (chatParticipant is null || chatParticipant.IsOwner) return;

            chatParticipant.IsOwner = true;

            _dbContext.ChatParticipants.Update(chatParticipant);
            await _dbContext.SaveChangesAsync();
        }

    }
}
