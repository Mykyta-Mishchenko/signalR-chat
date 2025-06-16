using Microsoft.AspNetCore.SignalR;

namespace chat_backend.Modules.OnlineChat
{
    public class ChatsHub : Hub
    {
        public override Task OnConnectedAsync()
        {


            return base.OnConnectedAsync();
        }
    }
}
