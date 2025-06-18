using chat_backend.Mappers;

namespace chat_backend.AppExtensionMethods
{
    public static class MappingExtension
    {
        public static void AddApplicationMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(ChatMappingProfile));
        }
    }
}
