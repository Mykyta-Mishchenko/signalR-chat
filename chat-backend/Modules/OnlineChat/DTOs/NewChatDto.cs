using chat_backend.Shared.Models;

namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class NewChatDto
    { 
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public ChatType ChatType { get; set; }
        public List<ChatFriendDto> Participants { get; set; } = new List<ChatFriendDto>();
    }
}
