using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace chat_backend.Shared.Data.DataModels
{
    public class ChatParticipant
    {
        [BindNever]
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public bool IsOwner { get; set; }

        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
