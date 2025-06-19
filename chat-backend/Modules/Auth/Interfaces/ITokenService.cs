using chat_backend.Shared.Data.DataModels;

namespace chat_backend.Modules.Auth.Interfaces.Services
{
    public interface ITokenService
    {
        public int RefreshTokenExpirationDays { get; }
        public Task<string> CreateAccessTokenAsync(User user);
        public string CreateRefreshToken();
        public void AppendRefreshToken(HttpContext httpContext, string refreshToken);
    }
}