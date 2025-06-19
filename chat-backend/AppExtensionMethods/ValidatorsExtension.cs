using chat_backend.Modules.Auth.Validators;
using chat_backend.Modules.Profile;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace chat_backend.AppExtensionMethods
{
    public static class ValidatorsExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<SignUpDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<SignInDTOValidator>();

            services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();
        }
    }
}
