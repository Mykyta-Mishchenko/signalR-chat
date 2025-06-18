using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Modules.OnlineChat.Interfaces.Services;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        public async Task<ChatMessageDto?> CreateMessageAsync(SendMessageDto sendedMessage)
        {
            var message = new Message
            {
                ChatId = sendedMessage.ChatId,
                SenderId = sendedMessage.SenderId,
                Content = sendedMessage.Content,
                TimeStamp = DateTime.Now
            };

            var dbMessage = await _messageRepository.CreateMessageAsync(message);

            return _mapper.Map<ChatMessageDto?>(dbMessage);
        }

        public async Task<List<ChatMessageDto>> GetChatMessagesAsync(int chatId)
        {
            var messages = await _messageRepository.GetChatMessagesAsync(chatId);
            return _mapper.Map<List<ChatMessageDto>>(messages);
        }

        public async Task<ChatMessageDto?> SetMessageAsReadAsync(int messageId)
        {
           var message =  await _messageRepository.SetMessageAsReadAsync(messageId);

            return _mapper.Map<ChatMessageDto?>(message);
        }
    }
}
