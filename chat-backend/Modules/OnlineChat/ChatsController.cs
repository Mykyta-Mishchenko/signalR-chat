﻿using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Services;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace chat_backend.Modules.OnlineChat
{
    [Route("api/chats")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IChatUserService _userService;
        private readonly IMessageService _messageService;
        public ChatsController(
            IChatService chatService,
            IChatUserService userService,
            IMessageService messageService)
        {
            _chatService = chatService;
            _userService = userService;
            _messageService = messageService;
        }
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserChats(int userId)
        {
            var chats = await _chatService.GetUserChatsWithParticipantsAsync(userId);
            return Ok(chats);
        }

        [HttpGet("find/user/{request}")]
        public async Task<IActionResult> FindUserByNameOrEmail(string request)
        {
            var users = await _userService.GetUsersByNameOrEmailAsync(request);
            return Ok(users);
        }

        [HttpGet("messages/chat/{chatId:int}")]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            var messages = await _messageService.GetChatMessagesAsync(chatId);
            return Ok(messages);
        }

        [HttpGet("info/chat/{chatId:int}")]
        public async Task<IActionResult> GetChatInfo(int chatId)
        {
            var userNameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(userNameIdentifier != null)
            {
                var userId = int.Parse(userNameIdentifier);
                var chatInfo = await _chatService.GetChatWithParticipantsAsync(chatId, userId);
                return Ok(chatInfo);
            }   

            return Unauthorized();
        }
    }
}
