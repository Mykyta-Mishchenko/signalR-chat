using System.Runtime.Serialization;

namespace chat_backend.Modules.Auth.Models
{
    public enum UserRole
    {
        [EnumMember(Value = "user")]
        user = 1,
        [EnumMember(Value = "admin")]
        admin = 2
    }
}
