namespace chat_backend.Modules.Auth.Models
{
    public class SessionTokens
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
