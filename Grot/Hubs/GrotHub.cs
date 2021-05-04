using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services;

namespace Grot.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GrotHub : Hub
    {
        public async Task SendProcessingDoneMessage(string message, string userId)
        {
            await Clients.User(userId).SendAsync("processingDoneReceived", message);
        }
    }
}
