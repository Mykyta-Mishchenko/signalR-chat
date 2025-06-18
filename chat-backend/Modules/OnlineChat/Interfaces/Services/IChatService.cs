using chat_backend.Modules.OnlineChat.DTOs;
using System.Numerics;

namespace chat_backend.Modules.OnlineChat.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<ChatInfoDto>> GetUserChatsWithParticipantsAsync(int userId);
        Task<ChatInfoDto?> GetChatWithParticipantsAsync(int chatId);
        Task<ChatInfoDto?> CreateChatWithParticipantsAsync(int creatorId, string name, List<int> participantsIds);
        Task SetChatOwnerAsync(int chatId, int ownerId);
    }
}
