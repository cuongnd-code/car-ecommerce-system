using Microsoft.AspNetCore.SignalR;

namespace ManageCars.Hubs
{
    public class CarHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


    }
}
