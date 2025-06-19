namespace chat_backend.Shared.Data.DataModels
{
    public class UserProfile
    {
        public int UserId {  get; set; }
        public string? ProfileImgUrl { get; set; }

        public User User { get; set; }
    }
}
