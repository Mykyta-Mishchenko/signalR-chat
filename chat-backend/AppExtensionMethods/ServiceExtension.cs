using chat_backend.Modules.Auth.Interfaces.Services;
using chat_backend.Modules.Auth.Services;
using chat_backend.Modules.Profile;

namespace monopoly_backend.AppExtensionMethods
{
    public static class ServiceExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProfileService, ProfileService>();
        }
    }
}
