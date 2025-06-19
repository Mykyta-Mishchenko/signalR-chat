using chat_backend.Modules.Auth.Interfaces.Repositories;
using chat_backend.Shared.Data;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace chat_backend.Modules.Auth.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ChatDbContext _dbContext;
        public RoleRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role?> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<OperationResult> DeleteRoleAsync(string roleName)
        {
            var dbRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (dbRole != null)
            {
                _dbContext.Roles.Remove(dbRole);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success;
            }

            return OperationResult.Success;
        }

        public async Task<bool> IsRoleExistsAsync(string roleName)
        {
            return (await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName)) != null;
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<IList<Role>> GetUserRolesAsync(User user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbUser != null)
            {
                return await _dbContext.UsersRoles
                .Where(ur => ur.UserId == dbUser.UserId)
                .Select(ur => ur.Role)
                .ToListAsync();
            }
            return new List<Role>();
        }

        public async Task<IList<Role>> GetUserRolesAsync(int userId)
        {
            return await _dbContext.UsersRoles
                .Where(ur=>ur.UserId == userId)
                .Select(ur=>ur.Role)
                .ToListAsync();
        }

        public async Task<UserRoles?> CreateUserRoleAsync(User user, string roleName)
        {
            var dbRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(dbRole != null && dbUser != null)
            {
                var dbUserRole = await _dbContext.UsersRoles
                    .FirstOrDefaultAsync(ur => (ur.UserId == dbUser.UserId && ur.RoleId == dbRole.RoleId));
                if(dbUserRole == null)
                {
                    var userRole = new UserRoles() { UserId = dbUser.UserId, RoleId = dbRole.RoleId };
                    await _dbContext.UsersRoles.AddAsync(userRole);
                    await _dbContext.SaveChangesAsync();
                    return userRole;
                }
            }

            return null;
        }

        public async Task<OperationResult> DeleteUserRoleAsync(User user, string roleName)
        {
            var dbRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (dbRole != null && dbUser != null)
            {
                var dbUserRole = await _dbContext.UsersRoles
                .FirstOrDefaultAsync(ur => (ur.UserId == dbUser.UserId && ur.RoleId == dbRole.RoleId));
                if (dbUserRole != null)
                {
                    _dbContext.UsersRoles.Remove(dbUserRole);
                    await _dbContext.SaveChangesAsync();
                    return OperationResult.Success;
                }
            }
            return OperationResult.Failure;
        }
    }
}
