using chat_backend.Modules.Auth.Models;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;

namespace chat_backend.Modules.Auth.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<SignUpResult> SignUpAsync(User user, UserRole role);
        public Task<SignInResult> SignInAsync(User user);
        public Task<OperationResult> Logout(string refreshToken);
        public Task<SessionTokens> RefreshSessionAsync(string refreshToken);
        public Task AddUserRoleAsync(User user, string roleName);
        public Task AddRoleAsync(string roleName);
    }
}
