namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class ChatParticipantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeenAt { get; set; }
    }
}
