using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Mappers
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile() {
            CreateMap<Chat, ChatInfoDto>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));

            CreateMap<ChatParticipant, ChatParticipantDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom(src => src.User.IsOnline))
                .ForMember(dest => dest.LastSeenAt, opt => opt.MapFrom(src => src.User.LastSeenAt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<User, ChatParticipantDto>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom(src => src.IsOnline))
                .ForMember(dest => dest.LastSeenAt, opt => opt.MapFrom(src => src.LastSeenAt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
