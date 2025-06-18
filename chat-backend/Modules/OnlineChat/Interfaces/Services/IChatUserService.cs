using chat_backend.Modules.OnlineChat.DTOs;
using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Interfaces.Services
{
    public interface IChatUserService
    {
        Task<List<ChatParticipantDto>> GetUsersByNameOrEmailAsync(string request);
        Task<User?> GetUserByIdAsync(int userId);
        Task SetUserOnlineAsync(int userId);
        Task SetUserOfflineAsync(int userId);
    }
}
