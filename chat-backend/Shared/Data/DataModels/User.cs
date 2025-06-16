using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace chat_backend.Shared.Data.DataModels
{
    public class User
    {
        [BindNever]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public UserProfile UserProfile { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection<RefreshSession> Sessions { get; set; }
    }
}
