using FluentValidation;

namespace chat_backend.Modules.Profile
{
    public class UserProfileValidator: AbstractValidator<UserProfileDTO>
    {
        public UserProfileValidator()
        {
            RuleFor(x => x.ProfileImg)
                .NotNull().WithMessage("File is required.")
                .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
                .Must(file => file.Length <= 2 * 1024 * 1024).WithMessage("Maximum file size is 2MB.")
                .Must(file =>
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    return allowedExtensions.Contains(extension);
                }).WithMessage("Only image files (.jpg, .jpeg, .png) are allowed.");
        }
    }
}
