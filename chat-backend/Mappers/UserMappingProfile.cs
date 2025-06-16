using chat_backend.Modules.Auth.DTO;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Mappers
{
    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<SignUpDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.Sessions, opt => opt.Ignore());

            CreateMap<SignInDTO, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.Sessions, opt => opt.Ignore());
        }
    }
}
