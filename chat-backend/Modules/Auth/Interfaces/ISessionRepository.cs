using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;

namespace chat_backend.Modules.Auth.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        public Task<RefreshSession?> CreateSessionAsync(RefreshSession session);

        public Task<IList<RefreshSession>> GetUserSessionsAsync(User user);
        public Task<RefreshSession?> GetUserSessionByTokenAsync(string refreshToken);

        public Task<RefreshSession?> UpdateSessionAsync(string oldRefreshToken, string newRefreshToken);

        public Task<OperationResult> DeleteSessionAsync(string refreshToken);
        public Task<OperationResult> DeleteUserSessionsAsync(User user);
    }
}
