namespace chat_backend.Modules.OnlineChat.Interfaces
{
    public interface IOnlineUsersRepository
    {
        Task SetUserOnlineAsync(int userId);
        Task SetUserOfflineAsync(int userId);
        Task<bool> IsUserOnlineAsync(int userId);
    }
}
