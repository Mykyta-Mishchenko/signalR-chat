using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Mappers
{
    public class ChatMessageMappingProfile : Profile
    {
        public ChatMessageMappingProfile() 
        {
            CreateMap<Message, ChatMessageDto>();
        }
    }
}
