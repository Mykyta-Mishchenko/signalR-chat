using chat_backend.Modules.Auth.Interfaces.Repositories;
using chat_backend.Modules.Auth.Interfaces.Services;
using chat_backend.Modules.Auth.Models;
using chat_backend.Shared.Data.DataModels;
using chat_backend.Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace chat_backend.Modules.Auth.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository, 
            IRoleRepository roleRepository,
            ISessionRepository sessionRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _sessionRepository = sessionRepository;
            _tokenService = tokenService;
        }
        public async Task<SignUpResult> SignUpAsync(User user, UserRole role)
        {
            var dbUser = await _userRepository.GetUserAsync(user.Email);
            if(dbUser == null)
            {
                try {
                    user.Password = HashPassword(user.Password);
                    await _userRepository.CreateUserAsync(user);
                    dbUser = await _userRepository.GetUserAsync(user.Email);

                    var roleName = role.ToString();

                    if (! await _roleRepository.IsRoleExistsAsync(roleName))
                    {
                        await _roleRepository.CreateRoleAsync(roleName);
                    }
                    await _roleRepository.CreateUserRoleAsync(dbUser, roleName);

                    await _userRepository.CreateUserProfileAsync(dbUser.UserId, "");

                    return SignUpResult.Success;
                }
                catch (Exception ex)
                {
                    return SignUpResult.Failure;
                }
            }

            return SignUpResult.EmailFailure;
        }

        public async Task<SignInResult> SignInAsync(User user)
        {
            var dbUser = await _userRepository.GetUserAsync(user.Email);
            if(dbUser != null)
            {
                if(dbUser.Password != HashPassword(user.Password))
                {
                    return SignInResult.Failure;
                }
                try
                {
                    var sessions = await _sessionRepository.GetUserSessionsAsync(dbUser);
                    if (sessions.Count() > 5)
                    {
                        await _sessionRepository.DeleteUserSessionsAsync(dbUser);
                    }
                    var session = new RefreshSession()
                    {
                        UserId = dbUser.UserId,
                        RefreshToken = _tokenService.CreateRefreshToken(),
                        ExpireTime = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays)
                    };
                    await _sessionRepository.CreateSessionAsync(session);
                    return SignInResult.SignedIn(
                        new SessionTokens()
                        {
                            RefreshToken = session.RefreshToken,
                            AccessToken = await _tokenService.CreateAccessTokenAsync(dbUser),
                        }
                    );
                }
                catch (Exception ex)
                {
                    return SignInResult.Failure;
                }
            }

            return SignInResult.NotAllowed;
        }

        public async Task<OperationResult> Logout(string refreshToken)
        {
            return await _sessionRepository.DeleteSessionAsync(refreshToken);
        }

        public async Task AddUserRoleAsync(User user, string roleName)
        {
            var dbUser = await _userRepository.GetUserAsync(user.Email);
            var roleExists = await _roleRepository.IsRoleExistsAsync(roleName);
            if (dbUser != null && roleExists)
            {
                await _roleRepository.CreateUserRoleAsync(dbUser, roleName);
            }
        }

        public async Task AddRoleAsync(string roleName)
        {
            var roleExists = await _roleRepository.IsRoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleRepository.CreateRoleAsync(roleName);
            }
        }

        public async Task<SessionTokens?> RefreshSessionAsync(string refreshToken)
        {
            var session = await _sessionRepository.GetUserSessionByTokenAsync(refreshToken);

            if (session != null)
            {
                var user = await _userRepository.GetUserAsync(session.UserId);

                var newRefreshToken = _tokenService.CreateRefreshToken();
                var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);
                await _sessionRepository.UpdateSessionAsync(refreshToken, newRefreshToken);

                return new SessionTokens()
                {
                    RefreshToken = newRefreshToken,
                    AccessToken = newAccessToken,
                };
            }

            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }
    }
}
