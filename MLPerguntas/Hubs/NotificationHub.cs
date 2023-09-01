using Microsoft.AspNetCore.SignalR;

namespace MLPerguntas.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Notify(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
