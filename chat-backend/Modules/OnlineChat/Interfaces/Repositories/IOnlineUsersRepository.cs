namespace chat_backend.Modules.OnlineChat.Interfaces.Repositories
{
    public interface IOnlineUsersRepository
    {
        Task SetUserOnlineAsync(int userId);
        Task SetUserOfflineAsync(int userId);
        Task<bool> IsUserOnlineAsync(int userId);
    }
}
