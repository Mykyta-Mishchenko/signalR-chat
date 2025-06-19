namespace chat_backend.Modules.Profile
{
    public interface IProfileService
    {
        public Task<string> SetUserProfileByRefreshTokenAsync(string refreshToken, IFormFile file);
        public Task<string> SetUserProfileByEmailTokenAsync(string email, IFormFile file);
        public Task<byte[]> GetUserProfileAsync(int userId);
    }
}
