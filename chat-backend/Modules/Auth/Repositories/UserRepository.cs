using chat_backend.Modules.Auth.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatDbContext _dbContext;
        public UserRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(int Id)
        {
            return await _dbContext.Users.FindAsync(Id);
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> GetUserProfileAsync(int userId)
        {
            var profile = await _dbContext.UsersProfile.FirstOrDefaultAsync(u => u.UserId == userId);
            return profile?.ProfileImgUrl ?? "";
        }

        public async Task<UserProfile?> CreateUserProfileAsync(int userId, string profileImgUrl)
        {
            try
            {
                var dbUserProfile = await _dbContext.UsersProfile.FirstOrDefaultAsync(p => p.UserId == userId);
                var newUserProfile = new UserProfile
                {
                    UserId = userId,
                    ProfileImgUrl = profileImgUrl
                };

                if (dbUserProfile == null)
                {
                    await _dbContext.UsersProfile.AddAsync(newUserProfile);
                    await _dbContext.SaveChangesAsync();
                    return newUserProfile;
                }
                else
                {
                    dbUserProfile.ProfileImgUrl = profileImgUrl;
                    await _dbContext.SaveChangesAsync();
                    return dbUserProfile;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OperationResult> DeleteUserAsync(User user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbUser != null)
            {
                _dbContext.Users.Remove(dbUser);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success;
            }

            return OperationResult.Failure;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbUser != null)
            {
                user.UserId = dbUser.UserId;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }
    }
}
