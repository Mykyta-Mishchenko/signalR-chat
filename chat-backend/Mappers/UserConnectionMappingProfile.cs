using AutoMapper;
using chat_backend.Modules.OnlineChat.Models;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Mappers
{
    public class UserConnectionMappingProfile : Profile
    {
        public UserConnectionMappingProfile() {
            CreateMap<User, UserConnection>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.ConnectionId));
        }
    }
}
