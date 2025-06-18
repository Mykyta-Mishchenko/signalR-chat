namespace chat_backend.Modules.OnlineChat.DTOs
{
    public class CreatePersonalChatRequestDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
    }
}
