using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> CreateChatWithParticipantsAsync(string chatName, List<int> participantsIds);
        Task<ChatParticipant?> AddParticipantToChatAsync(int chatId, int participantId);
    }
}
