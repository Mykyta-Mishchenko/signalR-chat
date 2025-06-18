namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
    }
}
