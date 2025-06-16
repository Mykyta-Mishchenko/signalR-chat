using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace chat_backend.Shared.Data.DataModels
{
    public class Role
    {
        [BindNever]
        public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<UserRoles> Roles { get; set; }
    }
}
