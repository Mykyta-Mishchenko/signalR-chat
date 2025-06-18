namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class ChatInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChatParticipantDto> Participants { get; set; } = new List<ChatParticipantDto>();
        public string LastMessage { get; set; }
        public DateTime LastMessageTimestamp { get; set; }
        public int UnreadMessagesCount { get; set; }
    }
}
