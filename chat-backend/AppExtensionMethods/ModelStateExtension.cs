using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace chat_backend.AppExtensionMethods
{
    public static class ModelStateExtension
    {
        public static List<string> GetAllErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(msg => !string.IsNullOrWhiteSpace(msg))
                .ToList();
        }
    }
}
