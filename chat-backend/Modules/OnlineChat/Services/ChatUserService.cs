using AutoMapper;
using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Modules.OnlineChat.Interfaces.Repositories;
using chat_backend.Modules.OnlineChat.Interfaces.Services;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Services
{
    public class ChatUserService : IChatUserService
    {
        private readonly IChatUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ChatUserService(
            IChatUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<ChatParticipantDto>> GetUsersByNameOrEmailAsync(string request)
        {
            var users = await _userRepository.GetUsersByNameOrEmail(request);
            return _mapper.Map<List<ChatParticipantDto>>(users);
        }
    }
}
