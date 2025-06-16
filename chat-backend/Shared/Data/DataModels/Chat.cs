using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace chat_backend.Shared.Data.DataModels
{
    public class Chat
    {
        [BindNever]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatParticipant> Participants { get; set; }
    }
}
