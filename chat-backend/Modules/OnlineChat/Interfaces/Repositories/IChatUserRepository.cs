using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.OnlineChat.Interfaces.Repositories
{
    public interface IChatUserRepository
    {
        Task<List<User>> GetUsersByNameOrEmail(string request);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
