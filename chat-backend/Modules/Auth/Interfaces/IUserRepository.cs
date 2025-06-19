using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;

namespace chat_backend.Modules.Auth.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<UserProfile?> CreateUserProfileAsync(int userId, string profileImgUrl);
        public Task<User?> CreateUserAsync(User user);

        public Task<User?> GetUserAsync(int Id);
        public Task<User?> GetUserAsync(string email);
        public Task<string> GetUserProfileAsync(int userId);

        public Task<User?> UpdateUserAsync(User user);

        public Task<OperationResult> DeleteUserAsync(User user);
        
    }
}
