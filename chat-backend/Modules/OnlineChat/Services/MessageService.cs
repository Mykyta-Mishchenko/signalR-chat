using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Modules.OnlineChat.Interfaces.Services;

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
        public async Task<List<ChatMessageDto>> GetChatMessagesAsync(int chatId)
        {
            var messages = await _messageRepository.GetChatMessagesAsync(chatId);
            return _mapper.Map<List<ChatMessageDto>>(messages);
        }
    }
}
