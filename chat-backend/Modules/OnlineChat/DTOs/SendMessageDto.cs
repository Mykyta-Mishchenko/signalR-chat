namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class SendMessageDto
    {
        public int ChatId { get; set; }
        public int SenderId {  get; set; }
        public string Content { get; set; }
    }
}
