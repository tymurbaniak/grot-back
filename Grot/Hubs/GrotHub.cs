using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Grot.Hubs
{
    public class GrotHub : Hub
    {
        public async Task SendProcessingDoneMessage(string message, string userId)
        {
            await Clients.User(userId).SendAsync("processingDoneReceived", message);
        }
    }
}
