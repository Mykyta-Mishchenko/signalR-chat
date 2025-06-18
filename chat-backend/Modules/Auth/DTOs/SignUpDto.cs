namespace chat_backend.Modules.Auth.DTO
{
    public class SignUpDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
