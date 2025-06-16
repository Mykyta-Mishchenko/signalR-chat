using chat_backend.Modules.Auth.Interfaces.Repositories;
using chat_backend.Modules.Auth.Repositories;

namespace chat_backend.AppExtensionMethods
{
    public static class RepositoryExtension
    {
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
        }
    }
}
