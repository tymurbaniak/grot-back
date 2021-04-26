using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grot.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.Name;
        }
    }
}
