using chat_backend.Modules.Auth.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.Auth.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ChatDbContext _dbContext;
        public SessionRepository (ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RefreshSession?> CreateSessionAsync(RefreshSession session)
        {
            await _dbContext.RefreshSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<IList<RefreshSession>> GetUserSessionsAsync(User user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbUser != null)
            {
                return await _dbContext.RefreshSessions
                    .Where(s => s.UserId == dbUser.UserId)
                    .ToListAsync();
            }
            return new List<RefreshSession> ();
        }

        public async Task<RefreshSession?> GetUserSessionByTokenAsync(string refreshToken)
        {
            return await _dbContext.RefreshSessions.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
        }

        public async Task<OperationResult> DeleteSessionAsync(string refreshToken)
        {
            var session = await _dbContext.RefreshSessions.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
            if(session != null)
            {
                _dbContext.RefreshSessions.Remove(session);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success;
            }

            return OperationResult.Failure;
        }

        public async Task<OperationResult> DeleteUserSessionsAsync(User user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbUser != null)
            {
                var sessions =  await _dbContext.RefreshSessions.Where(s => s.UserId == dbUser.UserId).ToListAsync();
                _dbContext.RefreshSessions.RemoveRange(sessions);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success;
            }

            return OperationResult.Failure;
        }

        public async Task<RefreshSession?> UpdateSessionAsync(string oldRefreshToken, string newRefreshToken)
        {
            var session = await _dbContext.RefreshSessions.FirstOrDefaultAsync(s=>s.RefreshToken == oldRefreshToken);
            if(session != null)
            {
                session.RefreshToken = newRefreshToken;
                _dbContext.RefreshSessions.Update(session);
                await _dbContext.SaveChangesAsync();
                return session;
            }
            return null;
        }
    }
}
