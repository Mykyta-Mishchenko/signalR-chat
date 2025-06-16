namespace chat_backend.Shared.Data.DataModels
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }

        public User Sender { get; set; }
        public Chat Chat { get; set; }
    }
}
