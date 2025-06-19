using System.Text;
using System.Security.Cryptography;
using Host = Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using chat_backend.Modules.Auth.Interfaces.Repositories;

namespace chat_backend.Modules.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly Host.IHostingEnvironment _environment;
        public ProfileService(
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            Host.IHostingEnvironment environment) 
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _environment = environment;
        }
        public async Task<byte[]> GetUserProfileAsync(int userId)
        {
            var filePath =  await _userRepository.GetUserProfileAsync(userId);
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }
            var imageBytes = File.ReadAllBytes(filePath);

            return imageBytes;
        }

        public async Task<string> SetUserProfileByEmailTokenAsync(string email, IFormFile file)
        {
            var user = await _userRepository.GetUserAsync(email);
            if(user == null)
            {
                return string.Empty;
            }
            var imgPath = await _userRepository.GetUserProfileAsync(user.UserId);

            if (imgPath != null)
            {
                DeletePreviousImg(imgPath);
            }

            return await SaveProfileImgAsync(user.UserId, file);
        }

        public async Task<string> SetUserProfileByRefreshTokenAsync(string refreshToken, IFormFile file)
        {
            var session = await _sessionRepository.GetUserSessionByTokenAsync(refreshToken);

            if (session != null)
            {
                var imgPath =  await _userRepository.GetUserProfileAsync(session.UserId);

                if(imgPath != null)
                {
                    DeletePreviousImg(imgPath);
                }

                return await SaveProfileImgAsync(session.UserId, file);
            }
            return string.Empty;
        }
        private void DeletePreviousImg(string imgPath)
        {
            if (File.Exists(imgPath))
            {
                File.Delete(imgPath);
            }
        }

        private async Task<string> SaveProfileImgAsync(int userId, IFormFile file)
        {
            var fileName = file.FileName + DateTime.Now.ToString();
            using (var sha = new SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(fileName);
                byte[] hash = sha.ComputeHash(textData);
                fileName = BitConverter.ToString(hash).Replace("-", "");
            }
            var filePath = Path.Combine(_environment.ContentRootPath, "ProfileImages", fileName);
            using (var image = await Image.LoadAsync<Rgba32>(file.OpenReadStream()))
            {
                int size = Math.Min(image.Width, image.Height);
                int x = (image.Width - size) / 2;
                int y = (image.Height - size) / 2;

                image.Mutate(ctx => ctx
                    .Crop(new Rectangle(x, y, size, size))
                    .Resize(256, 256));

                // Save with FileStream
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.SaveAsync(fileStream, new PngEncoder());
                }
            }
            await _userRepository.CreateUserProfileAsync(userId, filePath);
            return filePath;
        }
    }
}
