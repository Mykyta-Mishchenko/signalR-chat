using Azure.AI.TextAnalytics;
using chat_backend.Modules.OnlineChat.DTOs;

namespace chat_backend.Modules.OnlineChat.Interfaces.Services
{
    public interface IMessageService
    {
        Task<List<ChatMessageDto>> GetChatMessagesAsync(int chatId);
        Task<ChatMessageDto?> CreateMessageAsync(SendMessageDto sendedMessage);
        Task<ChatMessageDto?> CreateMessageWithSentimentAsync(SendMessageDto sendedMessage, TextSentiment sentiment);
        Task<ChatMessageDto?> SetMessageAsReadAsync(int messageId); 
    }
}
