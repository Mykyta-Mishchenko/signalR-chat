using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Services;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;

namespace chat_backend.Modules.OnlineChat
{
    [Authorize]
    public class ChatsHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IChatUserService _chatUserService;
        private readonly IMessageService _messageService;
        private readonly string CHAT_PREFIX = "Chat_";
        public ChatsHub(
            IChatService chatService, 
            IMessageService messageService,
            IChatUserService chatUserService)
        {
            _chatService = chatService;
            _messageService = messageService;
            _chatUserService = chatUserService;
        }
        public async override Task OnConnectedAsync()
        {
            var userIdentifier = Context.UserIdentifier;
            if(userIdentifier != null)
            {
                var userId = int.Parse(userIdentifier);
                var userChats = await _chatService.GetUserChatsAsync(userId);

                await _chatUserService.SetUserOnlineAsync(userId);

                foreach (var chat in userChats)
                {
                    if (chat.ChatType == ChatType.Personal)
                    {
                        await Clients.Group(CHAT_PREFIX + chat.Id).SendAsync("UserBecameOnline", userId);
                    }

                    await Groups.AddToGroupAsync(Context.ConnectionId, CHAT_PREFIX + chat.Id);
                }
            }
            await base.OnConnectedAsync();
        }

        public async Task SendMessageToChat(SendMessageDto message)
        {
            var dbMessage = await _messageService.CreateMessageAsync(message);

            if(dbMessage != null)
            {
                await Clients.Group(CHAT_PREFIX + dbMessage.ChatId).SendAsync("GetChatMessage", dbMessage);
            }
        }

        public  async Task ReadChatMessage(int messageId)
        {
            var message = await _messageService.SetMessageAsReadAsync(messageId);

            if(message != null)
            {
                await Clients.Group(CHAT_PREFIX + message.ChatId).SendAsync("MessageRead", message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdentifier = Context.UserIdentifier;
            if (userIdentifier != null)
            {
                var userId = int.Parse(userIdentifier);
                var userChats = await _chatService.GetUserChatsAsync(userId);

                await _chatUserService.SetUserOfflineAsync(userId);

                userChats = userChats.Where(c=>c.ChatType == ChatType.Personal).ToList();

                foreach (var chat in userChats)
                {
                    await Clients.Group(CHAT_PREFIX + chat.Id).SendAsync("UserBecameOffline", userId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
