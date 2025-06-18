using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Modules.OnlineChat.Interfaces.Services;
using chat_backend.Shared.Data.DataModels;
using Microsoft.Identity.Client;
using System;

namespace chat_backend.Modules.OnlineChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public ChatService(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<ChatInfoDto?> CreateChatWithParticipantsAsync(int creatorId, string name, List<int> participantsIds)
        {
            var chat = await _chatRepository.CreateChatWithParticipantsAsync(name, participantsIds);
            if(chat is null)
            {
                return null;
            }

            chat = await _chatRepository.GetChatInfoWithParticipantsAsync(chat.Id);

            if (chat is null)
            {
                return null;
            }

            chat.Name = GetChatName(chat, creatorId);
            await SetChatOwnerAsync(chat.Id, creatorId);

            return _mapper.Map<ChatInfoDto>(chat);
        }

        public async Task<ChatInfoDto?> GetChatWithParticipantsAsync(int chatId)
        {
            var chatInfo = await _chatRepository.GetChatInfoWithParticipantsAsync(chatId);

            return _mapper.Map<ChatInfoDto?>(chatInfo);
        }

        public async Task<List<ChatInfoDto>> GetUserChatsWithParticipantsAsync(int userId)
        {
            var chats = await _chatRepository.GetUserChatsWithParticipantsAsync(userId);
            var chatsInfo = new List<ChatInfoDto>();

            foreach(var chat in chats)
            {
                var lastMessage = await _messageRepository.GetLastChatMessageAsync(chat.Id);
                var unreadedMessages = await _messageRepository.GetUnreadedChatMessagesAsync(chat.Id);

                var chatInfo = new ChatInfoDto
                {
                    Id = chat.Id,
                    Name = GetChatName(chat, userId),
                    Participants = _mapper.Map<List<ChatParticipantDto>>(chat.Participants),
                    LastMessage = lastMessage?.Content ?? "",
                    LastMessageTimestamp = lastMessage?.TimeStamp ?? DateTime.MinValue,
                    UnreadMessagesCount = unreadedMessages.Count
                };

                chatsInfo.Add(chatInfo);
            }

            return chatsInfo.OrderByDescending(c => c.LastMessageTimestamp).ToList();
        }

        public async Task SetChatOwnerAsync(int chatId, int ownerId)
        {
            await _chatRepository.SetChatOwnerAsync(chatId, ownerId);
        }

        private string GetChatName(Chat chatWithParticipants, int userId)
        {
            if(chatWithParticipants.Participants.Count == 2)
            {
                return chatWithParticipants.Participants.FirstOrDefault(p => p.Id != userId)?.User.UserName ?? "";
            }

            if (string.IsNullOrEmpty(chatWithParticipants.Name))
            {
                foreach (var participant in chatWithParticipants.Participants)
                {
                    chatWithParticipants.Name += participant.User.UserName + " ";
                }
            }
            return chatWithParticipants.Name;
        }
    }
}
