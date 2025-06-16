using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> CreateMessageAsync(Message message);
        Task<Message?> SetMessageAsReadAsync(int messageId);
        Task<Message?> GetLastChatMessageAsync(int chatId);
    }
}
