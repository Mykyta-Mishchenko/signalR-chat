using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace chat_backend.Shared.Data.DataModels
{
    public class RefreshSession
    {
        [BindNever]
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireTime { get; set; }

        public User User { get; set; }
    }
}
