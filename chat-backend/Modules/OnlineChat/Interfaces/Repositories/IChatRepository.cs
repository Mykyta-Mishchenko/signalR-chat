using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Interfaces.Repositories
{
    public interface IChatRepository
    { 
        Task<Chat?> CreateChatWithParticipantsAsync(string chatName, List<int> participantsIds);
        Task<List<Chat>> GetUserChatsWithParticipantsAsync(int userId);
        Task<Chat?> GetChatInfoWithParticipantsAsync(int chatId);
        Task<ChatParticipant?> AddParticipantToChatAsync(int chatId, int participantId);
        Task SetChatOwnerAsync(int chatId, int ownerId);
    }
}
