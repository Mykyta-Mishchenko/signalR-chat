using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Shared.Data.DataModels;
using System.Numerics;

namespace chat_backend.Modules.OnlineChat.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<Chat>> GetUserChatsAsync(int userId);
        Task<List<ChatInfoDto>> GetUserChatsWithParticipantsAsync(int userId);
        Task<ChatInfoDto?> GetChatWithParticipantsAsync(int chatId);
        Task<ChatInfoDto?> CreateChatWithParticipantsAsync(int creatorId, Chat chat, List<int> participantsIds);
        Task SetChatOwnerAsync(int chatId, int ownerId);
        Task SetChatOwnersByChatTypeAsync(ChatInfoDto chat, int creatorId);
    }
}
